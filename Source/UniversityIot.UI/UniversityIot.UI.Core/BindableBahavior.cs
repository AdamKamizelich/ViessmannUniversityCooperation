﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace UniversityIot.UI.Core
{
    public class BindableBehavior<T> : Behavior<T>
          where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            this.AssociatedObject = visualElement;

            if (visualElement.BindingContext != null)
            {
                this.BindingContext = visualElement.BindingContext;
            }

            visualElement.BindingContextChanged += this.OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            this.OnBindingContextChanged();
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= this.OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.BindingContext = this.AssociatedObject.BindingContext;
        }
    }

    public class EventToCommandBehavior : BindableBehavior<View>
    {
        public static readonly BindableProperty EventNameProperty =
            BindableProperty.Create<EventToCommandBehavior, string>(p => p.EventName, null);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<EventToCommandBehavior, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<EventToCommandBehavior, object>(p => p.CommandParameter, null);

        public static readonly BindableProperty EventArgsConverterProperty =
            BindableProperty.Create<EventToCommandBehavior, IValueConverter>(p => p.EventArgsConverter, null);

        public static readonly BindableProperty EventArgsConverterParameterProperty =
            BindableProperty.Create<EventToCommandBehavior, object>(p => p.EventArgsConverterParameter, null);

        private EventInfo _eventInfo;
        private Delegate _handler;

        public string EventName
        {
            get
            {
                return (string)this.GetValue(EventNameProperty);
            }
            set
            {
                this.SetValue(EventNameProperty, value);
            }
        }

        public ICommand Command
        {
            get
            {
                return (ICommand)this.GetValue(CommandProperty);
            }
            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        public object CommandParameter
        {
            get
            {
                return this.GetValue(CommandParameterProperty);
            }
            set
            {
                this.SetValue(CommandParameterProperty, value);
            }
        }

        public IValueConverter EventArgsConverter
        {
            get
            {
                return (IValueConverter)this.GetValue(EventArgsConverterProperty);
            }
            set
            {
                this.SetValue(EventArgsConverterProperty, value);
            }
        }

        public object EventArgsConverterParameter
        {
            get
            {
                return this.GetValue(EventArgsConverterParameterProperty);
            }
            set
            {
                this.SetValue(EventArgsConverterParameterProperty, value);
            }
        }

        protected override void OnAttachedTo(View visualElement)
        {
            base.OnAttachedTo(visualElement);

            EventInfo[] events = this.AssociatedObject.GetType().GetRuntimeEvents().ToArray();
            if (events.Any())
            {
                this._eventInfo = events.FirstOrDefault(e => e.Name == this.EventName);
                if (this._eventInfo == null)
                {
                    throw new ArgumentException(String.Format("EventToCommand: Can't find any event named '{0}' on attached type",
                                                              this.EventName));
                }

                this.AddEventHandler(this._eventInfo, this.AssociatedObject, this.OnFired);
            }
        }

        protected override void OnDetachingFrom(View view)
        {
            if (this._handler != null)
            {
                this._eventInfo.RemoveEventHandler(this.AssociatedObject, this._handler);
            }

            base.OnDetachingFrom(view);
        }

        private void AddEventHandler(EventInfo eventInfo, object item, Action<object, EventArgs> action)
        {
            ParameterExpression[] eventParameters = eventInfo.EventHandlerType
                                                             .GetRuntimeMethods().First(m => m.Name == "Invoke")
                                                             .GetParameters()
                                                             .Select(p => Expression.Parameter(p.ParameterType))
                                                             .ToArray();

            MethodInfo actionInvoke = action.GetType()
                                            .GetRuntimeMethods().First(m => m.Name == "Invoke");

            this._handler = Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
                eventParameters
                )
                                      .Compile();

            eventInfo.AddEventHandler(item, this._handler);
        }

        private void OnFired(object sender, EventArgs eventArgs)
        {
            if (this.Command == null)
            {
                return;
            }

            object parameter = this.CommandParameter;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                parameter = eventArgs;

                if (this.EventArgsConverter != null)
                {
                    parameter = this.EventArgsConverter.Convert(eventArgs, typeof(object), this.EventArgsConverterParameter,
                                                                CultureInfo.CurrentUICulture);
                }
            }

            if (this.Command.CanExecute(parameter))
            {
                this.Command.Execute(parameter);
            }
        }
    }
}

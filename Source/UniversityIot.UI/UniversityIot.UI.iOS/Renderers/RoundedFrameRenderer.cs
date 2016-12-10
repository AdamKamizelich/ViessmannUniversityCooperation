using System;
using UniversityIot.UI.Core.Controls;
using UniversityIot.UI.iOS.Renderers;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(RoundedFrame), typeof(RoundedFrameRenderer))]

namespace UniversityIot.UI.iOS.Renderers
{
    using Xamarin.Forms.Platform.iOS;

    class RoundedFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            var element = (RoundedFrame) this.Element;
            if (element != null)
            {
                this.Layer.CornerRadius = (nfloat) element.CornerRadius;

                if (this.Layer.Sublayers.Length > 0)
                {
                    var subLayer = this.Layer.Sublayers[0];
                    subLayer.CornerRadius = (nfloat)element.CornerRadius;
                    subLayer.MasksToBounds = true;
                }
            }
        }
    }
}
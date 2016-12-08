using System;

namespace ImprovedContentManager.Redirection.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class RedirectReverseAttribute : RedirectAttribute
    {
        public RedirectReverseAttribute() : base(false)
        {
        }

        public RedirectReverseAttribute(bool onCreated) : base(onCreated)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using FreshMvvm;
using ReactiveUI;
using PropertyChangingEventArgs = ReactiveUI.PropertyChangingEventArgs;
using PropertyChangingEventHandler = ReactiveUI.PropertyChangingEventHandler;

namespace Cosmosshirts.PageModel
{
    public class BasePageModel : FreshBasePageModel, IReactiveObject
    {
        private readonly ReactiveObject reactiveObject;
        public event PropertyChangingEventHandler PropertyChanging;

        public BasePageModel()
        {
       
        }
        public IObservable<Exception> ThrownExceptions => reactiveObject.ThrownExceptions;
        public event ReactiveUI.PropertyChangingEventHandler propertychanging
        {
            add => reactiveObject.PropertyChanging += value;
            remove => reactiveObject.PropertyChanging -= value;
        }
        public void RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            reactiveObject.RaisePropertyChanged(args.PropertyName);
        }


        public void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            
            base.RaisePropertyChanged(args.PropertyName);
        }

    }
}

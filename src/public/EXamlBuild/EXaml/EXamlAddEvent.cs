﻿using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Tizen.NUI.Binding;
using Tizen.NUI.Xaml.Build.Tasks;

namespace Tizen.NUI.EXaml
{
    //use ##
    internal class EXamlAddEvent : EXamlOperation
    {
        internal override string Write()
        {
            if (false == Instance.IsValid)
            {
                return "";
            }

            string ret = "";

            ret += "#";

            ret += String.Format("({0} {1} {2} {3})", 
                GetValueString(Instance),
                GetValueString(Element),
                GetValueString(definedEvents.GetIndex(eventDef.DeclaringType, eventDef)),
                GetValueString(definedMethods.GetIndex(Value.DeclaringType, Value)));

            ret += "#\n";

            return ret;
        }

        public EXamlAddEvent(EXamlCreateObject instance, EXamlCreateObject element, string eventName, MethodDefinition value)
        {
            TypeReference typeref;
            var eventDef = instance.Type.GetEvent(fi=>fi.Name==eventName, out typeref);
            if (null != eventDef)
            {
                Instance = instance;
                Element = element;
                Value = value;
                DeclaringType = typeref;

                this.eventDef = eventDef;

                Instance.AddEvent(DeclaringType, eventDef);

                EXamlOperation.eXamlOperations.Add(this);
                eXamlAddEventList.Add(this);
            }
            else
            {
                throw new Exception("Property is not element");
            }
        }

        internal static List<EXamlAddEvent> eXamlAddEventList
        {
            get;
        } = new List<EXamlAddEvent>();

        internal EXamlCreateObject Instance
        {
            get;
        }

        internal EXamlCreateObject Element
        {
            get;
        }

        internal TypeReference DeclaringType
        {
            get;
        }

        internal MethodDefinition Value
        {
            get;
        }

        private EventDefinition eventDef;
    }
}

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplestDI
{
    public class SimplestContainer
    {
        private SortedList<string, Type> container = new SortedList<string, Type>();

        public void Register<T>()
        {
            container.Add(typeof(T).FullName, typeof(T));
        }

        public T Create<T>()
        {
            return GetInstance<T>(container[typeof(T).FullName]);
        }

        private T GetInstance<T>(Type t)
        {
            // check for dependencies in constructor methods
            ConstructorInfo[] ci = t.GetConstructors();
            foreach (ConstructorInfo info in ci)
            {
                // look thru the parameters of each constructor
                ParameterInfo[] pi = info.GetParameters();
                foreach (ParameterInfo p in pi)
                {
                    // check if a constructor parameter is of non-primitive type to rule them out
                    var paramType = p.ParameterType;
                    if (!paramType.IsPrimitive)
                    {
                        // found possible dependency - check against container 
                        // TODO - only supports one parameter/dependency for now
                        Type DependsOn = null;                        
                        if (container.TryGetValue(paramType.FullName, out DependsOn)) {
                            // yes its a dependency we are managing so lets instantiate that
                            var DependsOnObj = System.Activator.CreateInstance(DependsOn);

                            // INJECT that DEPENDENCY in
                            return (T)System.Activator.CreateInstance(t, DependsOnObj);
                        }
                    }
                    
                }
            }

            // no dependencies - the default simplest instance to be returned 
            return (T) System.Activator.CreateInstance(t);
        }
    }
}

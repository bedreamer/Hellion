using Hellion.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hellion.Core.Network
{
    public static class FFPacketHandler
    {
        private static Dictionary<object, MethodInfo> registeredPackets;

        /// <summary>
        /// Initialize the FFPacketHeadler fields and properties.
        /// </summary>
        static FFPacketHandler()
        {
            registeredPackets = new Dictionary<object, MethodInfo>();
        }

        /// <summary>
        /// Load the methods of the T object containing all methods with the FFIncomingPacketAttribute using reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Initialize<T>()
        {
            var typeInfo = typeof(T).GetTypeInfo();
            MethodInfo[] allMethods = typeInfo.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            var methodsWithAttribute = from x in allMethods
                                       where x.GetCustomAttribute<FFIncomingPacketAttribute>() != null
                                       select x;

            foreach (var method in methodsWithAttribute)
            {
                var attribute = method.GetCustomAttribute<FFIncomingPacketAttribute>();

                if (registeredPackets.ContainsKey(attribute.Header))
                    throw new Exception("The key " + attribute.Header.ToString() + " already exists");

                registeredPackets.Add(attribute.Header, method);
            }
        }

        /// <summary>
        /// Invoke the right method corresponding to the header passed as parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">Object instance</param>
        /// <param name="header">Packet header</param>
        /// <param name="parameter">Packet parameter</param>
        /// <returns></returns>
        public static bool Invoke<T>(T instance, object header, object parameter)
        {
            if (!registeredPackets.ContainsKey(header))
                return false;

            try
            {
                registeredPackets[header].Invoke(instance, new object[] { parameter });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                Log.Error("Inner message {0}", e.InnerException.Message);
                Log.Error(e.StackTrace);
                return false;
            }

            return true;
        }
    }
}

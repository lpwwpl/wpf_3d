﻿using System;
using System.Collections.Generic;


/// <summary>
/// Provides a mechanism to be add and locate a specific services
/// </summary>
namespace CoreMVVM.Services
{
    /// <summary>
    /// This class acts as a resolver for typed services 
    /// (interfaces and implementations).
    /// </summary>
    /// <example>
    /// To register a service use Add:
    /// <![CDATA[
    /// serviceResolver.Add(typeof(IService), new Service());
    /// 
    /// To retrieve a service use Resolve:
    /// 
    /// IService svc = serviceResolver<IService>.Resolve();
    /// ]]>
    /// </example>
    public class ServiceProvider : IServiceProvider
    {

        /*
        #region Data
        private readonly Dictionary<Type, object> _services = 
            new Dictionary<Type, object>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears all services from the resolver list 
        /// </summary>
        public void Clear()
        {
            if (_services != null && _services.Count > 0)
                _services.Clear();
        }

        /// <summary>
        /// Adds a new service to the resolver list
        /// </summary>
        /// <param name="type">Service Type (typically an interface)</param>
        /// <param name="value">Object that implements service</param>
        public void Add(Type type, object value)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (value == null)
                throw new ArgumentNullException("value");

            lock (_services)
            {
                // Replacing existing service
                if (_services.ContainsKey(type))
                    _services[type] = value;
                // Adding new service
                else
                    _services.Add(type, value);
            }
        }

        /// <summary>
        /// Remove a service
        /// </summary>
        /// <param name="type">Type to remove</param>
        public void Remove(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            lock (_services)
            {
                _services.Remove(type);
            }
        }

        /// <summary>
        /// This resolves a service type and returns the implementation. Note that this
        /// assumes the key used to register the object is of the appropriate type or
        /// this method will throw an InvalidCastException!
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Implementation</returns>
        public T Resolve<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Implementation of IServiceProvider
        /// </summary>
        /// <param name="serviceType">Service Type</param>
        /// <returns>Object implementing service</returns>
        public object GetService(Type serviceType)
        {
            lock (_services)
            {
                object value;
                return _services.TryGetValue(serviceType, out value) ? value : null;
            }
        }
        #endregion
         */

        Dictionary<Type, object> services = new Dictionary<Type, object>();

        #region IServiceProvider Members

        /// <summary>
        /// Gets a service from the service locator
        /// </summary>
        /// <typeparam name="T">The type of service you want to get</typeparam>
        /// <returns>Returns the instance of the service</returns>
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        /// <summary>
        /// Registers a service to the service locator
        /// </summary>
        /// <param name="serviceType">The type of service to register. This is used so that you can register the service by an interface that the object implements</param>
        /// <param name="service">The service to add</param>
        /// <param name="overwriteIfExists">Passing true will replace any existing service</param>
        /// <returns>Returns true if the service was successfully registered</returns>
        /// <remarks>
        ///     <para>This generics based implementation ensures that the service must at least inherit from the service type.</para>
        ///     <para>NOTE: the MSDN documentation on IServiceProvidor states that the GetService method returns an object of type servieProvider</para>
        /// </remarks>
        public bool RegisterService<T>(T service, bool overwriteIfExists)
        {
            if (service == null)
            {
                throw new ArgumentNullException("Service");
            }
            lock (services)
            {
                if (!services.ContainsKey(typeof(T)))
                {
                    services.Add(typeof(T), service);
                    return true;
                }
                else if (overwriteIfExists)
                {
                    services[typeof(T)] = service;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Registers a service to the service locator. This will overwrite any registered services with the same registration type
        /// </summary>
        /// <param name="serviceType">The type of service to register. This is used so that you can register the service by an interface that the object implements</param>
        /// <param name="service">The service to add</param>
        /// <returns>Returns true if the service was successfully registered</returns>
        /// <remarks>
        ///     <para>This generics based implementation ensures that the service must at least inherit from the service type.</para>
        ///     <para>NOTE: the MSDN documentation on IServiceProvidor states that the GetService method returns an object of type servieProvider</para>
        /// </remarks>
        public bool RegisterService<T>(T service)
        {
            return RegisterService<T>(service, false);
        }

        /// <summary>
        /// Gets a service from the service locator
        /// </summary>
        /// <param name="serviceType">The type of service you want to get</param>
        /// <returns>Returns the instance of the service</returns>
        /// <remarks>This implements IServiceProvider</remarks>
        public object GetService(Type serviceType)
        {
            lock (services)
            {
                if (services.ContainsKey(serviceType))
                    return services[serviceType];
            }
            return null;
        }

        /// <summary>
        /// Remove a service
        /// </summary>
        /// <param name="type">Type to remove</param>
        public void Remove(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            lock (services)
            {
                services.Remove(type);
            }
        }

        /// <summary>
        /// Clears all services from the resolver list 
        /// </summary>
        public void Clear()
        {
            if (services != null && services.Count > 0)
                services.Clear();
        }
        #endregion

    }
}

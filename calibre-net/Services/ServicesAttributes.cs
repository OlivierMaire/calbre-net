// using System.Security.Cryptography.X509Certificates;

// namespace Calibre_net.Services;

// public class ScopedRegistrationAttribute : Attribute { }

// public class SingletonRegistrationAttribute : Attribute { }

// public class TransientRegistrationAttribute : Attribute { }


// public static class ServiceExtensions
// {
//     /// <summary>
//     /// Register all services decorated with *Registration Attributes.
//     /// </summary>
//     /// <param name="services"></param>
//     /// <param name="configuration"></param>
//     public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
//     {
//         // Define types that need matching
//         Type scopedRegistration = typeof(ScopedRegistrationAttribute);
//         Type singletonRegistration = typeof(SingletonRegistrationAttribute);
//         Type transientRegistration = typeof(TransientRegistrationAttribute);

//         var types = AppDomain.CurrentDomain.GetAssemblies()
//                   .SelectMany(s => s.GetTypes())
//                   .Where(p => p.IsDefined(scopedRegistration, false) || p.IsDefined(transientRegistration, false)
//                   || p.IsDefined(singletonRegistration, false) && !p.IsInterface)
//                   .Select(s => new
//                   {
//                       Service = s.GetInterface($"I{s.Name}"),
//                       Implementation = s
//                   })
//                   ;
//         //.Where(x => x.Service != null);

//         foreach (var type in types)
//         {
//             if (type.Implementation.IsDefined(scopedRegistration, false))
//                 _ = type.Service == null ? services.AddScoped(type.Implementation) : services.AddScoped(type.Service, type.Implementation);

//             if (type.Implementation.IsDefined(transientRegistration, false))
//                 _ = type.Service == null ? services.AddTransient(type.Implementation) : services.AddTransient(type.Service, type.Implementation);

//             if (type.Implementation.IsDefined(singletonRegistration, false))
//                 _ = type.Service == null ? services.AddSingleton(type.Implementation) : services.AddSingleton(type.Service, type.Implementation);
//         }
//     }
// }
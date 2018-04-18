using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;

namespace EmguCV.Workbench.Util
{
    /// <summary>
    /// Static utility class with misc. functions.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets a collection of MEF exported types.
        /// </summary>
        /// <typeparam name="T">The exported type.</typeparam>
        /// <param name="catalog">The assembly catalog containing the exported parts.</param>
        /// <returns>A collection of exported types.</returns>
        public static IEnumerable<Type> GetExportedTypes<T>(AssemblyCatalog catalog)
        {
            return catalog.Parts
                .Select(part => ComposablePartExportType<T>(part))
                .Where(t => t != null);
        }

        /// <summary>
        /// Composes the type from a composable part defenition.
        /// </summary>
        /// <typeparam name="T">The exported type.</typeparam>
        /// <param name="part">The composable part definition.</param>
        /// <returns>The type from the composable part definition.</returns>
        private static Type ComposablePartExportType<T>(ComposablePartDefinition part)
        {
            return part.ExportDefinitions.Any(
                def => def.Metadata.ContainsKey("ExportTypeIdentity") &&
                       def.Metadata["ExportTypeIdentity"].Equals(typeof(T).FullName))
                ? ReflectionModelServices.GetPartType(part).Value
                : null;
        }
    }
}
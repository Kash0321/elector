using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.CrossCutting.Diagnostics
{
    /// <summary>
    /// Comprobaciones estáticas de valores de parámetros
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Comprobación de que el valor de un parámetro de tipo <see cref="string"/> no es Null o cadena vacía
        /// </summary>
        /// <param name="parameter">Valor del parámetro</param>
        /// <param name="parameterName">Nombre del parámetro</param>
        /// <exception cref="ArgumentNullException">Cuando el valor del parámetro es Null o <see cref="string.Empty"/></exception>
        public static void NotEmpty(string parameter, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Comprobación de que el valor de un parámetro no es Null 
        /// </summary>
        /// <param name="parameter">Valor del parámetro</param>
        /// <param name="parameterName">Nombre del parámetro</param>
        /// <exception cref="ArgumentNullException">Cuando el valor del parámetro es null</exception>
        public static void NotNull(object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}

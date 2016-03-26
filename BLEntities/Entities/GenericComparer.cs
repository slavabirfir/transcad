using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BLEntities.Entities
{
    // <summary>
    /// Reflection based, Generic Comparer
    /// class for in-line or instance based Sorting
    /// </summary>
    /// <example>
    /// List<DataObject> array = new List<DataObject>();
    /// 
    /// ... Code to populate list ...
    /// 
    /// array.sort(new GenericComparer<DataObject>("ID", "asc", null));
    /// 
    /// or
    /// 
    /// GenericComparer genericComparer = 
    ///       new GenericComparer<DataObject>();
    /// genericComparer.MemberName = "ID";
    /// genericComparer.SortOrder = "asc";
    /// 
    /// array.sort(genericComparer);
    /// </example>
    public class GenericComparer<T> : IComparer<T>
    {
        #region Fields
        private string memberName = string.Empty;
        private string sortOrder = string.Empty;
        private List<object> methodParameters = new List<object>();
        PropertyInfo propertyInfo = null;
        MethodInfo methodInfo = null;
        #endregion

        #region Properties
        public string MemberName
        {
            get
            {
                return memberName;
            }
            set
            {
                memberName = value;
                GetReflected();
            }
        }

        public string SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                sortOrder = value;
            }
        }

        public List<object> MethodParameters
        {
            get
            {
                return methodParameters;
            }
        }
        #endregion

        #region Ctors
        /// <summary>
        /// Default constructor for use in instances.
        /// </summary>
        public GenericComparer()
        {
        }

        /// <summary>
        /// Constructor for in-line instantiation
        /// </summary>
        /// <param name="memberName">string of the member name to use for comparison
        /// can be either a method name or a property.
        /// </param>
        /// <param name="sortOrder">string of the sort order (case independent). 
        /// "ASC" for ascending order or anyting else for descending order</param>
        /// <param name="methodParameters">object array
        ///      of parameters to use for method, null otherwise.</param>
        public GenericComparer(string memberName, string sortOrder,
               List<object> methodParameters)
        {
            this.memberName = memberName;
            this.sortOrder = sortOrder;
            this.methodParameters = methodParameters;

            GetReflected();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Sets the global field, propertyInfo
        /// and/or memberInfo using the underlying Type
        /// </summary>
        private void GetReflected()
        {
            Type[] underlyingTypes = this.GetType().GetGenericArguments();
            Type thisUnderlyingtype = underlyingTypes[0];

            MemberInfo[] mi = thisUnderlyingtype.GetMember(memberName);
            if (mi.Length > 0)
            {
                if (mi[0].MemberType == MemberTypes.Property)
                {
                    propertyInfo = thisUnderlyingtype.GetProperty(memberName);
                }
                else if (mi[0].MemberType == MemberTypes.Method)
                {
                    Type[] signatureTypes = new Type[0];
                    if (methodParameters != null && methodParameters.Count > 0)
                    {
                        signatureTypes = new Type[methodParameters.Count];
                        for (int i = 0; i < methodParameters.Count; i++)
                        {
                            signatureTypes[i] = methodParameters[i].GetType();
                        }
                        methodInfo =
                          thisUnderlyingtype.GetMethod(memberName,
                          signatureTypes);
                    }
                    else
                    {
                        methodInfo =
                          thisUnderlyingtype.GetMethod(memberName,
                          signatureTypes);
                    }
                }
                else
                {
                    throw new Exception("Member name: " + memberName +
                          " is not a Public Property or " +
                          "a Public Method in Type: " +
                          thisUnderlyingtype.Name + ".");
                }
            }
            else
            {
                throw new Exception("Member name: " +
                          memberName + " not found.");
            }
        }

        /// <summary>
        /// Return an IComparable for use in the Compare method
        /// </summary>
        /// <param name="obj">object to get IComparable from</param>
        /// <returns>IComparable for this object</returns>
        private IComparable GetComparable(T obj)
        {
            if (methodInfo != null)
            {
                return (IComparable)methodInfo.Invoke(obj,
                                    methodParameters.ToArray());
            }
            else
            {
                return (IComparable)propertyInfo.GetValue(obj, null);
            }

        }
        #endregion

        #region IComparer Implementation
        /// <summary>
        /// Implementing method for IComparer
        /// </summary>
        /// <param name="objOne">Object to compare from</param>
        /// <param name="objTwo">Object to compare to</param>
        /// <returns>int of the comparison, or 0 if equal</returns>
        public int Compare(T objOne, T objTwo)
        {
            IComparable iComparable1 = GetComparable(objOne);
            IComparable iComparable2 = GetComparable(objTwo);

            if (sortOrder != null && sortOrder.ToUpper().Equals("ASC"))
            {
                if (iComparable1 == null)
                    return 1;
                else
                    return iComparable1.CompareTo(iComparable2);
            }
            else
            {
                if (iComparable1 == null)
                    return 1;
                else
                    return iComparable2.CompareTo(iComparable1);
            }
        }
        #endregion
    }
}

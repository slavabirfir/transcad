using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Utilities;
using System.Data.Objects.DataClasses;
using BLEntities.Accessories;
using System.ComponentModel;
using System.Collections;

namespace BLEntities.Entities
{
    public class BaseClassStyle
    {
        public String BackColor { get; set; }
        public String ForeColor { get; set; }
    }
    
    /// <summary>
    /// Delegate to BLManager to define custom validation rules
    /// </summary>
    /// <param name="vr"></param>
    /// <param name="baseClass"></param>
    public delegate void ValidationFunctionDelegate(ValidationResults vr, BaseClass baseClass);

    public abstract class BaseClass : IEditableObject

    {
        #region IEditableObject Members
        
        
        protected virtual void BeginEditImplementation()
        {}
        protected virtual void  CancelEditImplementation()
        {}
        protected virtual void EndEditImplementation()
        {}
        public void BeginEdit()
        {
            BeginEditImplementation();
        }

        public void CancelEdit()
        {
            CancelEditImplementation(); 

        }

        public  void EndEdit()
        {
            EndEditImplementation();
        }

        public virtual void ArchiveObject()
        { 
        }

        #endregion


        /// <summary>
        /// Validation Errors
        /// </summary>
        private List<ValidationErrorResult> validationErrors = null; 
        public List<ValidationErrorResult> ValidationErrors {
            get
            {
                return validationErrors;
            }
        }

        #region Base properties 
        /// <summary>
        /// ID
        /// </summary>
        [RangeValidator(1, RangeBoundaryType.Inclusive, 1000000000, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "ID_0")]
        public int ID { get; set; }

        [RegexValidator(".+$", MessageTemplateResourceType = typeof(Resources.ValidationResource)
            , MessageTemplateResourceName = "Name_Empty")]
        public string Name { get; set; }


        public bool IsNewEntity { get; set; }
        public int IdDuplicatedSourceID { get; set; }
        public string DisticntValue { get; set; }

        #endregion

        /// <summary>
        /// Validation Function from BL Manager
        /// </summary>
        public ValidationFunctionDelegate ValidationFunction;
        /// <summary>
        /// Base Class Style
        /// </summary>
        public virtual BaseClassStyle BaseClassStyle { get { return null; } }
        /// <summary>
        /// Check Integrity
        /// </summary>
        /// <param name="results"></param>
        [SelfValidation]
        public void CheckIntegrity(ValidationResults results)
        {
            if (ValidationFunction != null)
            {
                ValidationFunction(results,this);
            }
        }
        /// <summary>
        /// Validate
        /// </summary>
        /// <returns></returns>
        public void Validate()
        {
            var validator = ValidationFactory.CreateValidator(this.GetType());
            var results = validator.Validate(this);
            var lstResults = new List<ValidationErrorResult>();
            if (!results.IsValid)
            {
                 
                foreach (ValidationResult result in results)
                {
                    lstResults.Add(new ValidationErrorResult
                    {
                        ErrorMessage = result.Message,
                        PropertyName = result.Key
                    });
                }
                validationErrors = lstResults;
            }
            else
                validationErrors = null;
            
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

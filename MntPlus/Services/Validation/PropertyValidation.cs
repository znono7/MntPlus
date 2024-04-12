﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public abstract class PropertyValidation : BaseViewModel, IDataErrorInfo
    {
        #region Fields 
         
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// This holds the list of validation results and controls when the validation should be 
        /// performed and if the validation is valid.
        /// </summary>
        private Dictionary<string, bool> _validationResults { get; set; } = new Dictionary<string, bool>();

        #endregion

        #region public

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <param name="value">The property value.</param>
        public void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);

            SetValue<T>(propertyName, value);
        }
        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property value.</param>
        public void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            _values[propertyName] = value;
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        public T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);

            return GetValue<T>(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        public T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

        /// <summary>
        /// Validates current instance properties using Data Annotations.
        /// </summary>
        /// <param name="propertyName">This instance property to validate.</param>
        /// <returns>Relevant error string on validation failure or <see cref="System.String.Empty"/> on validation success.</returns>
        public virtual string OnValidate(string propertyName)
        {
            string error = string.Empty;

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            //Check if the Field has been added, this keeps track of when the validation
            //is performed.
            if (_validationResults.Any(x => x.Key == propertyName))
            {
                var value = GetValue(propertyName);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>(1);
                var result = Validator.TryValidateProperty(
                      value,
                      new ValidationContext(this, null, null)
                      {
                          MemberName = propertyName
                      },
               results);
                if (!result)
                {
                    var validationResult = results.First();
                    error = validationResult.ErrorMessage;

                    //Store a true result in the validation to set the error.
                    _validationResults[propertyName] = true;
                }
                else
                {
                    //If the Validation has been run and not invalid make sure the 
                    //paramter in the list is cleared, otherwise validation would 
                    //always return invalid once it is invalidated.
                    _validationResults[propertyName] = false;
                }
            }
            else
            {
                //This is the first run of the Validation, simply store the paramter
                //in the validation list and wait until next time to validate.
                _validationResults.Add(propertyName, true);
            }

            //Notify that things have changed
            OnPropertyChanged("IsValid");

            //Return the actual result
            return error;
        }

        #endregion

        #region Public

        /// <summary>
        /// This returns if the Validation is Valid or not
        /// </summary>
        /// <returns>True if the Validation has been perfomed and if there are not 
        /// true values. Will return false until the validation has been done once.</returns>
        public bool IsValid
        {
            get { return (!_validationResults.Any(x => x.Value) && (_validationResults.Count > 0)); }
        }

        /// <summary>
        /// Clears/Reset the Validation
        /// </summary>
        public void ClearValidation() 
        {
            _validationResults.Clear();
        }

        #endregion
         
        #region Change Notification

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public void OnPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                string propertyName = GetPropertyName(propertySelector);
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // IOnPropertyChanged Members

        #region Data Validation

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }

        #endregion

        #region Privates

        private string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }
        private object GetValue(string propertyName)
        {
            object value;
            if (!_values.TryGetValue(propertyName, out value))
            {
                var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
                if (propertyDescriptor == null)
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }

                value = propertyDescriptor.GetValue(this);
                _values.Add(propertyName, value);
            }

            return value;
        }

        #endregion
        #region Debugging

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }
        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        public virtual bool ThrowOnInvalidPropertyName { get; private set; }

        #endregion // Debugging Aides
    }

  

}

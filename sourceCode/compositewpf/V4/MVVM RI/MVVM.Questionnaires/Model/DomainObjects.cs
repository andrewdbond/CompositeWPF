//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===================================================================================
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MVVM.Questionnaires.Framework;

namespace MVVM.Questionnaires.Model
{
    #region pseudo generated code - code for domain classes as it could have been generated by a tool

    /// <summary>
    /// The MultipleSelectionQuestion domain object class.
    /// </summary>
    public sealed partial class MultipleSelectionQuestion : Question
    {
        private ICollection<string> response;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleSelectionQuestion"/> class.
        /// </summary>
        public MultipleSelectionQuestion()
        {
        }

        /// <summary>
        /// Gets or sets the Response value.
        /// </summary>
        [CustomValidation(typeof(CommonValidation), "ValidationMultipleSelectionResponseChoices")]
        [Required()]
        public ICollection<string> Response
        {
            get
            {
                return this.response;
            }

            set
            {
                if (this.response != value)
                {
                    this.ValidateProperty("Response", value);
                    this.response = value;
                    this.RaisePropertyChanged("Response");
                }
            }
        }
    }

    /// <summary>
    /// The MultipleSelectionQuestionTemplate domain object class.
    /// </summary>
    public sealed partial class MultipleSelectionQuestionTemplate : QuestionTemplate
    {
        private int? maxSelections;

        private string[] range;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleSelectionQuestionTemplate"/> class.
        /// </summary>
        public MultipleSelectionQuestionTemplate()
        {
        }

        /// <summary>
        /// Gets or sets the MaxSelections value.
        /// </summary>
        public int? MaxSelections
        {
            get
            {
                return this.maxSelections;
            }

            set
            {
                if (this.maxSelections != value)
                {
                    this.ValidateProperty("MaxSelections", value);
                    this.maxSelections = value;
                    this.RaisePropertyChanged("MaxSelections");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Range value.
        /// </summary>
        public string[] Range
        {
            get
            {
                return this.range;
            }

            set
            {
                if (this.range != value)
                {
                    this.ValidateProperty("Range", value);
                    this.range = value;
                    this.RaisePropertyChanged("Range");
                }
            }
        }
    }

    /// <summary>
    /// The NumericQuestion domain object class.
    /// </summary>
    public sealed partial class NumericQuestion : Question
    {
        private int? response;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericQuestion"/> class.
        /// </summary>
        public NumericQuestion()
        {
        }

        /// <summary>
        /// Gets or sets the Response value.
        /// </summary>
        [CustomValidation(typeof(CommonValidation), "ValidationNumericQuestionResponseRange")]
        [Required()]
        public int? Response
        {
            get
            {
                return this.response;
            }

            set
            {
                if (this.response != value)
                {
                    this.ValidateProperty("Response", value);
                    this.response = value;
                    this.RaisePropertyChanged("Response");
                }
            }
        }
    }

    /// <summary>
    /// The NumericQuestionTemplate domain object class.
    /// </summary>
    public sealed partial class NumericQuestionTemplate : QuestionTemplate
    {
        private int? maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericQuestionTemplate"/> class.
        /// </summary>
        public NumericQuestionTemplate()
        {
        }

        /// <summary>
        /// Gets or sets the MaxValue value.
        /// </summary>
        public int? MaxValue
        {
            get
            {
                return this.maxValue;
            }

            set
            {
                if (this.maxValue != value)
                {
                    this.ValidateProperty("MaxValue", value);
                    this.maxValue = value;
                    this.RaisePropertyChanged("MaxValue");
                }
            }
        }
    }

    /// <summary>
    /// The OpenQuestion domain object class.
    /// </summary>
    public sealed partial class OpenQuestion : Question
    {
        private string response;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenQuestion"/> class.
        /// </summary>
        public OpenQuestion()
        {
        }

        /// <summary>
        /// Gets or sets the Response value.
        /// </summary>
        [CustomValidation(typeof(CommonValidation), "ValidationOpenQuestionResponseLength")]
        [Required()]
        public string Response
        {
            get
            {
                return this.response;
            }

            set
            {
                if (this.response != value)
                {
                    this.ValidateProperty("Response", value);
                    this.response = value;
                    this.RaisePropertyChanged("Response");
                }
            }
        }
    }

    /// <summary>
    /// The OpenQuestionTemplate domain object class.
    /// </summary>
    public sealed partial class OpenQuestionTemplate : QuestionTemplate
    {
        private int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenQuestionTemplate"/> class.
        /// </summary>
        public OpenQuestionTemplate()
        {
        }

        /// <summary>
        /// Gets or sets the MaxLength value.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }

            set
            {
                if (this.maxLength != value)
                {
                    this.ValidateProperty("MaxLength", value);
                    this.maxLength = value;
                    this.RaisePropertyChanged("MaxLength");
                }
            }
        }
    }

    /// <summary>
    /// The Question domain object class.
    /// </summary>
    public abstract partial class Question : DomainObject
    {
        private QuestionTemplate questionTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Question"/> class.
        /// </summary>
        protected Question()
        {
        }

        /// <summary>
        /// Gets or sets the associated <see cref="QuestionTemplate"/> entity.
        /// </summary>
        public QuestionTemplate QuestionTemplate
        {
            get
            {
                return this.questionTemplate;
            }

            set
            {
                if (this.questionTemplate != value)
                {
                    this.ValidateProperty("QuestionTemplate", value);
                    this.questionTemplate = value;
                    this.RaisePropertyChanged("QuestionTemplate");
                }
            }
        }
    }

    /// <summary>
    /// The Questionnaire domain object class.
    /// </summary>
    public sealed partial class Questionnaire : DomainObject
    {
        private int? age;

        private string name;

        private ICollection<Question> questions;

        private QuestionnaireTemplate template;

        /// <summary>
        /// Initializes a new instance of the <see cref="Questionnaire"/> class.
        /// </summary>
        public Questionnaire()
        {
        }

        /// <summary>
        /// Gets or sets the Age value.
        /// </summary>
        [Range(21, 100)]
        public int? Age
        {
            get
            {
                return this.age;
            }

            set
            {
                if (this.age != value)
                {
                    this.ValidateProperty("Age", value);
                    this.age = value;
                    this.RaisePropertyChanged("Age");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        [Required()]
        [StringLength(50)]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name != value)
                {
                    this.ValidateProperty("Name", value);
                    this.name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets the collection of associated <see cref="Question"/> entities.
        /// </summary>
        public ICollection<Question> Questions
        {
            get
            {
                if (this.questions == null)
                {
                    this.questions = new List<Question>();
                }

                return this.questions;
            }
        }

        /// <summary>
        /// Gets or sets the associated <see cref="QuestionnaireTemplate"/> entity.
        /// </summary>
        public QuestionnaireTemplate Template
        {
            get
            {
                return this.template;
            }

            set
            {
                if (this.template != value)
                {
                    this.ValidateProperty("Template", value);
                    this.template = value;
                    this.RaisePropertyChanged("Template");
                }
            }
        }
    }

    /// <summary>
    /// The QuestionnaireTemplate domain object class.
    /// </summary>
    public sealed partial class QuestionnaireTemplate : DomainObject
    {
        private ICollection<QuestionTemplate> questions;

        private string title;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireTemplate"/> class.
        /// </summary>
        public QuestionnaireTemplate()
        {
        }

        /// <summary>
        /// Gets the collection of associated <see cref="QuestionTemplate"/> entities.
        /// </summary>
        public ICollection<QuestionTemplate> Questions
        {
            get
            {
                if (this.questions == null)
                {
                    this.questions = new List<QuestionTemplate>();
                }

                return this.questions;
            }
        }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.ValidateProperty("Title", value);
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
    }

    /// <summary>
    /// The QuestionTemplate domain object class.
    /// </summary>
    public abstract partial class QuestionTemplate : DomainObject
    {
        private string questionText;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionTemplate"/> class.
        /// </summary>
        protected QuestionTemplate()
        {
        }

        /// <summary>
        /// Gets or sets the QuestionText value.
        /// </summary>
        public string QuestionText
        {
            get
            {
                return this.questionText;
            }

            set
            {
                if (this.questionText != value)
                {
                    this.ValidateProperty("QuestionText", value);
                    this.questionText = value;
                    this.RaisePropertyChanged("QuestionText");
                }
            }
        }
    }

    /// <summary>
    /// The QuestionnaireTemplateSummary domain object class.
    /// </summary>
    public sealed partial class QuestionnaireTemplateSummary : DomainObject
    {
        private string title;

        private string description;

        private int numberOfQuestions;

        private float estimatedTime;

        private int timesTaken;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireTemplateSummary"/> class.
        /// </summary>
        public QuestionnaireTemplateSummary()
        {
        }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.ValidateProperty("Title", value);
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description != value)
                {
                    this.ValidateProperty("Description", value);
                    this.description = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets or sets the NumberOfQuestions value.
        /// </summary>
        public int NumberOfQuestions
        {
            get
            {
                return this.numberOfQuestions;
            }

            set
            {
                if (this.numberOfQuestions != value)
                {
                    this.ValidateProperty("NumberOfQuestions", value);
                    this.numberOfQuestions = value;
                    this.RaisePropertyChanged("NumberOfQuestions");
                }
            }
        }

        /// <summary>
        /// Gets or sets the EstimatedTime value.
        /// </summary>
        public float EstimatedTime
        {
            get
            {
                return this.estimatedTime;
            }

            set
            {
                if (this.estimatedTime != value)
                {
                    this.ValidateProperty("EstimatedTime", value);
                    this.estimatedTime = value;
                    this.RaisePropertyChanged("EstimatedTime");
                }
            }
        }

        /// <summary>
        /// Gets or sets the TimesTaken value.
        /// </summary>
        public int TimesTaken
        {
            get
            {
                return this.timesTaken;
            }

            set
            {
                if (this.timesTaken != value)
                {
                    this.ValidateProperty("TimesTaken", value);
                    this.timesTaken = value;
                    this.RaisePropertyChanged("TimesTaken");
                }
            }
        }
    }

    #endregion
}
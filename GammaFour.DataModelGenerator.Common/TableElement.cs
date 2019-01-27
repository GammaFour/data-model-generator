﻿// <copyright file="TableElement.cs" company="Gamma Four, Inc.">
//     Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// A description of a table.
    /// </summary>
    public class TableElement : XElement, IComparable<TableElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableElement"/> class.
        /// </summary>
        /// <param name="xElement">The element that describes the table.</param>
        public TableElement(XElement xElement)
            : base(xElement)
        {
            // Extract the name from the schema.
            this.Name = this.Attribute(XmlSchemaDocument.ObjectName).Value;

            // The verbs tell us what actions to support in the controller when it's built.
            List<Verb> verbs = new List<Verb>();
            XAttribute verbAttribute = this.Attribute(XmlSchemaDocument.Verbs);
            string verbStrings = verbAttribute == null ? string.Empty : verbAttribute.Value;
            foreach (string verbString in verbStrings.Split(','))
            {
                if (Enum.TryParse<Verb>(verbString, out Verb verb))
                {
                    verbs.Add(verb);
                }
            }

            this.Verbs = verbs.ToArray();

            // This will navigate to the sequence of columns.
            XElement complexType = this.Element(XmlSchemaDocument.ComplexType);
            XElement sequence = complexType.Element(XmlSchemaDocument.Sequence);

            // Every table has an implicit row version column to track changes.
            sequence.Add(
                new XElement(
                    XmlSchemaDocument.Element,
                    new XAttribute("name", "RowVersion"),
                    new XAttribute(XName.Get("IsRowVersion", "urn:schemas-gamma-four-com:xml-gfdata"), "true"),
                    new XAttribute("type", "xs:base64Binary")));

            // This will replace each of the undecorated elements with decorated ones.
            List<XElement> columnElements = sequence.Elements(XmlSchemaDocument.Element).ToList();
            foreach (XElement columnElement in columnElements)
            {
                sequence.Add(new ColumnElement(columnElement));
                columnElement.Remove();
            }
        }

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public List<ColumnElement> Columns
        {
            get
            {
                XElement complexType = this.Element(XmlSchemaDocument.ComplexType);
                XElement sequence = complexType.Element(XmlSchemaDocument.Sequence);
                return sequence.Elements(XmlSchemaDocument.Element).Cast<ColumnElement>().ToList();
            }
        }

        /// <summary>
        /// Gets the ForeignKey constraints.
        /// </summary>
        public List<ForeignKeyElement> ForeignKeys
        {
            get
            {
                return (from fke in this.XmlSchemaDocument.ForeignKeys
                        where fke.UniqueKey.Table == this
                        select fke).ToList();
            }
        }

        /// <summary>
        /// Gets the index of this table in the list of tables.
        /// </summary>
        public int Index
        {
            get
            {
                return this.XmlSchemaDocument.Tables.IndexOf(this);
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets an indication whether the table is written to a persistent store.
        /// </summary>
        public bool IsPersistent { get; private set; } = true;

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Gets the foreign keys which are children of this table.
        /// </summary>
        public List<ForeignKeyElement> ChildKeys
        {
            get
            {
                return (from fke in this.XmlSchemaDocument.ForeignKeys
                        where fke.UniqueKey.Table == this
                        orderby fke.Name
                        select fke).ToList();
            }
        }

        /// <summary>
        /// Gets the foreign keys which are the parents of this table.
        /// </summary>
        public List<ForeignKeyElement> ParentKeys
        {
            get
            {
                return (from fke in this.XmlSchemaDocument.ForeignKeys
                        where fke.Table == this
                        orderby fke.Name
                        select fke).ToList();
            }
        }

        /// <summary>
        /// Gets the primary key on this table.
        /// </summary>
        public UniqueKeyElement PrimaryKey
        {
            get
            {
                return (from uk in this.UniqueKeys
                        where uk.IsPrimaryKey
                        select uk).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the unique constraints.
        /// </summary>
        public List<UniqueKeyElement> UniqueKeys
        {
            get
            {
                return (from uke in this.XmlSchemaDocument.UniqueKeys
                        where uke.Table == this
                        orderby uke.Name
                        select uke).ToList();
            }
        }

        /// <summary>
        /// Gets the owner document.
        /// </summary>
        public XmlSchemaDocument XmlSchemaDocument
        {
            get
            {
                return this.Document as XmlSchemaDocument;
            }
        }

        /// <summary>
        /// Gets or sets the verbs supported by the controller.
        /// </summary>
        public Verb[] Verbs { get; set; }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(TableElement other)
        {
            return this.Name.CompareTo(other.Name);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
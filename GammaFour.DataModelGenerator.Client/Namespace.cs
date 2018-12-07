﻿// <copyright file="Namespace.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// The root namespace.
    /// </summary>
    public class Namespace
    {
        /// <summary>
        /// The data model schema.
        /// </summary>
        private XmlSchemaDocument xmlSchemaDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="Namespace"/> class.
        /// </summary>
        /// <param name="xmlSchemaDocument">The name of the namespace.</param>
        public Namespace(XmlSchemaDocument xmlSchemaDocument)
        {
            // Initialize the object.
            this.xmlSchemaDocument = xmlSchemaDocument;

            // This is the syntax of the namespace.
            this.Syntax = SyntaxFactory.NamespaceDeclaration(
                    SyntaxFactory.IdentifierName(xmlSchemaDocument.TargetNamespace))
                .WithUsings(SyntaxFactory.List<UsingDirectiveSyntax>(this.UsingStatements))
                .WithMembers(this.Members)
                .WithLeadingTrivia(this.LeadingTrivia)
                .WithTrailingTrivia(this.TrailingTrivia);
        }

        /// <summary>
        /// Gets or sets gets the syntax.
        /// </summary>
        public NamespaceDeclarationSyntax Syntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the documentation comment.
        /// </summary>
        private SyntaxTriviaList LeadingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>();

                // // <auto-generated />
                trivia.Add(
                    SyntaxFactory.Comment("// <auto-generated />"));

                // #pragma warning disable SA1402
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))));

                // #pragma warning disable SA1649
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia).NormalizeWhitespace();
            }
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        private SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                // Create the members.
                SyntaxList<MemberDeclarationSyntax> members = default(SyntaxList<MemberDeclarationSyntax>);
                members = this.CreatePublicInterfaces(members);
                members = this.CreatePublicClasses(members);
                return members;
            }
        }

        /// <summary>
        /// Gets the trailing trivia.
        /// </summary>
        private SyntaxTriviaList TrailingTrivia
        {
            get
            {
                // The document comment trivia is collected in this list.
                List<SyntaxTrivia> trivia = new List<SyntaxTrivia>();

                // #pragma warning restore SA1402
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1402")))));

                // #pragma warning restore SA1649
                trivia.Add(
                    SyntaxFactory.Trivia(
                        SyntaxFactory.PragmaWarningDirectiveTrivia(
                            SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                            true)
                        .WithErrorCodes(
                            SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                                SyntaxFactory.IdentifierName("SA1649")))));

                // This is the complete document comment.
                return SyntaxFactory.TriviaList(trivia).NormalizeWhitespace();
            }
        }

        /// <summary>
        /// Gets the 'using' statements.
        /// </summary>
        private List<UsingDirectiveSyntax> UsingStatements
        {
            get
            {
                // Create the 'using' statements.
                // [TODO] Make the addition of user namespaces part of the initialization.  Run through the tables and extract the
                // namespaces from the types found therein.
                List<UsingDirectiveSyntax> usingStatements = new List<UsingDirectiveSyntax>();
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.Generic")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Collections.ObjectModel")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ComponentModel")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Diagnostics.CodeAnalysis")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Linq")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ServiceModel")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ServiceModel.Channels")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.ServiceModel.Description")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System.Threading.Tasks")));
                usingStatements.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("GammaFour.ClientModel")));
                return usingStatements;
            }
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicClasses(SyntaxList<MemberDeclarationSyntax> members)
        {
            // The class for accessing the data service.
            members = members.Add(new DataServiceClient.Class(this.xmlSchemaDocument).Syntax);

            // Create the row classes.
            List<RowClass.Class> rowClasses = new List<RowClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                rowClasses.Add(new RowClass.Class(tableElement));
            }

            // Alphabetize the list of row classes and add them to the namespace.
            foreach (RowClass.Class rowClass in rowClasses.OrderBy(c => c.Name))
            {
                members = members.Add(rowClass.Syntax);
            }

            // Create the list of tables.
            List<TableClass.Class> tableClasses = new List<TableClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                tableClasses.Add(new TableClass.Class(tableElement));
            }

            // Alphabetize the list of tables and add them to the namespace.
            foreach (TableClass.Class @class in tableClasses.OrderBy(c => c.Name))
            {
                members = members.Add(@class.Syntax);
            }

            // The actual data model class.
            members = members.Add(new DataModelClass.Class(this.xmlSchemaDocument).Syntax);

            // Create the compound key sets.
            List<Common.CompoundKeyStruct.Struct> compoundKeys = new List<Common.CompoundKeyStruct.Struct>();

            // Create a compound key when there are more than one columns.
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
                {
                    // If a key has more than one column, then we need a compound key structure to use it.
                    if (uniqueKeyElement.Columns.Count > 1)
                    {
                        compoundKeys.Add(new Common.CompoundKeyStruct.Struct(uniqueKeyElement));
                    }
                }
            }

            // Alphabetize the list of compound keys and add them to the namespace.
            foreach (Common.CompoundKeyStruct.Struct @struct in compoundKeys.OrderBy(c => c.Name))
            {
                members = members.Add(@struct.Syntax);
            }

            // Create the unique index classes.
            List<UniqueKeyIndexClass.Class> uniqueIndexClasses = new List<UniqueKeyIndexClass.Class>();
            foreach (TableElement tableElement in this.xmlSchemaDocument.Tables)
            {
                foreach (UniqueKeyElement uniqueKeyElement in tableElement.UniqueKeys)
                {
                    uniqueIndexClasses.Add(new UniqueKeyIndexClass.Class(uniqueKeyElement));
                }
            }

            // Alphabetize the list of unique indices and add them to the namespace.
            foreach (UniqueKeyIndexClass.Class @class in uniqueIndexClasses.OrderBy(c => c.Name))
            {
                members = members.Add(@class.Syntax);
            }

            // Create the list of foreign indices.
            List<ForeignKeyIndexClass.Class> foreignIndexClasses = new List<ForeignKeyIndexClass.Class>();
            foreach (ForeignKeyElement foreignKeyElement in this.xmlSchemaDocument.ForeignKeys)
            {
                foreignIndexClasses.Add(new ForeignKeyIndexClass.Class(foreignKeyElement));
            }

            // Alphabetize the list of foreign indices and add them to the namespace.
            foreach (ForeignKeyIndexClass.Class @class in foreignIndexClasses.OrderBy(c => c.Name))
            {
                members = members.Add(@class.Syntax);
            }

            // This is the collection of alphabetized fields.
            return members;
        }

        /// <summary>
        /// Creates the classes.
        /// </summary>
        /// <param name="members">The collection of members.</param>
        /// <returns>The collection of members augmented with the classes.</returns>
        private SyntaxList<MemberDeclarationSyntax> CreatePublicInterfaces(SyntaxList<MemberDeclarationSyntax> members)
        {
            // The interface for accessing the data service.
            members = members.Add(new DataServiceInterface.Interface(this.xmlSchemaDocument).Syntax);

            // This is the collection of alphabetized fields.
            return members;
        }
    }
}
﻿// <copyright file="EnlistAndLockStatements.cs" company="Gamma Four, Inc.">
//    Copyright © 2018 - Gamma Four, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.DataModelGenerator.RestService
{
    using System.Collections.Generic;
    using GammaFour.DataModelGenerator.Common;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Creates a code block to throw a null argument exception.
    /// </summary>
    public static class EnlistAndLockStatements
    {
        /// <summary>
        /// Gets the syntax for the creation of an anonymous type.
        /// </summary>
        /// <param name="tableElement">The description of a table.</param>
        /// <param name="isDecorated">Indicates that the argument name should be prefixed with the index name.</param>
        /// <returns>An expression that builds an anonymous type from a table description.</returns>
        public static List<StatementSyntax> GetSyntax(TableElement tableElement, bool isDecorated = false)
        {
            // This is used to collect the statements.
            List<StatementSyntax> statements = new List<StatementSyntax>();

            //                    this.domain.Provinces.Enlist();
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.ThisExpression(),
                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToCamelCase())),
                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                            SyntaxFactory.IdentifierName("Enlist")))));

            //                    this.domain.Provinces.ProvinceExternalKey.Enlist();
            //                    this.domain.Provinces.ProvinceKey.Enlist();
            foreach (UniqueKeyElement innerUniqueKeyElement in tableElement.UniqueKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Enlist")))));
            }

            //                    this.domain.Provinces.CountryProvinceKey.Enlist();
            //                    this.domain.Provinces.RegionProvinceRegionIdKey.Enlist();
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.ThisExpression(),
                                            SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToCamelCase())),
                                        SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                SyntaxFactory.IdentifierName("Enlist")))));
            }

            //                    await this.domain.Provinces.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            statements.Add(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AwaitExpression(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName(tableElement.XmlSchemaDocument.Name.ToCamelCase())),
                                                SyntaxFactory.IdentifierName(tableElement.Name.ToPlural())),
                                            SyntaxFactory.IdentifierName("Lock")),
                                        SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.ThisExpression(),
                                                    SyntaxFactory.IdentifierName("lockTimeout")))))),
                                SyntaxFactory.IdentifierName("ConfigureAwait")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.FalseLiteralExpression))))))));

            //                    await this.domain.Provinces.ProvinceExternalKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Provinces.ProvinceKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (UniqueKeyElement innerUniqueKeyElement in tableElement.UniqueKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(innerUniqueKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                                        SyntaxFactory.IdentifierName(innerUniqueKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(innerUniqueKeyElement.Name)),
                                                SyntaxFactory.IdentifierName("Lock")),
                                            SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("lockTimeout")))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            //                    await this.domain.Provinces.CountryProvinceKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Provinces.RegionProvinceRegionIdKey.Lock.EnterWriteLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (ForeignKeyElement foreignKeyElement in tableElement.ParentKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(foreignKeyElement.XmlSchemaDocument.Name.ToCamelCase())),
                                                        SyntaxFactory.IdentifierName(foreignKeyElement.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(foreignKeyElement.Name)),
                                                SyntaxFactory.IdentifierName("Lock")),
                                            SyntaxFactory.IdentifierName("EnterWriteLockAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("lockTimeout")))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            //                    await this.domain.Countries.CountryKey.Lock.EnterReadLockAsync(this.lockTimeout).ConfigureAwait(false);
            //                    await this.domain.Regions.RegionKey.Lock.EnterReadLockAsync(this.lockTimeout).ConfigureAwait(false);
            foreach (ForeignKeyElement parentKeyElement in tableElement.ParentKeys)
            {
                statements.Add(
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AwaitExpression(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            SyntaxFactory.ThisExpression(),
                                                            SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.XmlSchemaDocument.Name.ToCamelCase())),
                                                        SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Table.Name.ToPlural())),
                                                    SyntaxFactory.IdentifierName(parentKeyElement.UniqueKey.Name)),
                                                SyntaxFactory.IdentifierName("Lock")),
                                            SyntaxFactory.IdentifierName("EnterReadLockAsync")))
                                    .WithArgumentList(
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.ThisExpression(),
                                                        SyntaxFactory.IdentifierName("lockTimeout")))))),
                                    SyntaxFactory.IdentifierName("ConfigureAwait")))
                            .WithArgumentList(
                                SyntaxFactory.ArgumentList(
                                    SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                        SyntaxFactory.Argument(
                                            SyntaxFactory.LiteralExpression(
                                                SyntaxKind.FalseLiteralExpression))))))));
            }

            // This set of statement will enlist the indices and tables in the current transaction and acquire an exclusive lock.
            return statements;
        }
    }
}
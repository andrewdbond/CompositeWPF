//===============================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Automation;
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.TabItems;
using Core;
using Core.UIItems.WindowItems;
using StockTraderRI.AcceptanceTests.TestInfrastructure;
using StockTraderRI.AcceptanceTests.Helpers;
using StockTraderRI.AcceptanceTests.TestInfrastructure.MockModels;
using StockTraderRI.AcceptanceTests.TestInfrastructure.DataProvider.ModuleDataProviders;
using Core.UIItems.ListBoxItems;
using System.Globalization;


namespace StockTraderRI.AcceptanceTests.AutomatedTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    [DeploymentItem(@".\StockTraderRI\bin\Debug")]
    [DeploymentItem(@".\StockRI.Tests.AcceptanceTests\bin\Debug")]
    public class NewsModuleFixture : FixtureBase
    {
        [TestInitialize()]
        public void MyTestInitialize()
        {
            base.TestInitialize();
        }

        /// <summary>
        /// TestCleanup performs clean-up activities after each test method execution
        /// </summary>
        [TestCleanup()]
        public void MyTestCleanup()
        {
            base.TestCleanup();
        }

        /// <summary>
        /// Check if Selected symbol's news button click adds the tab with  
        /// title as a symbol code.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Click on the news button of a symbol detail
        /// 3. Check the new tab with symbol code as a title is added to the tab control.
        /// 
        /// Expected Result:
        /// On clicking the News button of a selected symbol, new tab with title as a symbol
        /// code should be created.
        /// </summary>
        [TestMethod]
        public void SelectedSymbolNewsDisplay()
        {
            //get handle of the Position Table and click on each of the rows one after the other
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            string symbol;
            List<News> news;
            ListBox articlesView;
            Label articleTitle;
            Label articlePublishedDate;

            foreach (ListViewRow symbolRow in list.Rows)
            {                
                symbolRow.Click();
                symbol = symbolRow.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text;

                news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(symbol);
                articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));
                
                if (null != news)
                {
                    Assert.AreEqual<int>(news.Count, articlesView.Items.Count);
                }
                else
                {
                    Assert.AreEqual<int>(0, articlesView.Items.Count);
                }

                foreach (News newsItem in news)
                {
                    articleTitle = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("ArticleTitle"))
                                                            .AndByText(newsItem.Title)
                                                            .AndControlType(typeof(Label)));

                    Assert.IsNotNull(articleTitle);

                    articlePublishedDate = (Label)Window.Get(SearchCriteria.ByAutomationId(TestDataInfrastructure.GetControlId("ArticlePublishedDate"))
                                                            .AndByText(newsItem.PublishedDate.ToString())
                                                            .AndControlType(typeof(Label)));

                    Assert.IsNotNull(articlePublishedDate);
                }
            }
        }

        /// <summary>
        /// Check on repeated click of a particular symbol in the position table  should not
        /// add listitems every click.
        ///
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get a particular row of the position table of a particular symbol
        /// 3. Click on the particular position table's row.
        /// 4. Check the new list box is added with the news articles.
        /// 5. Click on the particular position table's row.
        /// 6. Check the new news articles are not added.
        /// 
        /// Expected Result:
        /// Only for the first click new news articles should be added for a particular symbol.
        /// </summary>
        [TestMethod]
        public void SelectedSymbolRepeatedPositionRowClick()
        {
             //get handle of the Position Table 
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            string symbol;
            List<News> news;
            ListBox articlesView;

            //click the particular symbol of the Position table
            ListViewRow symbolRow = list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("Symbol")));
            symbolRow.Click();

            //Get the news item for a particular symbol
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");           
            news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(symbol);

            //Get the handler of the News Article View displayed
            articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));

            //Get the number of news items for the particular symbol
            int newsCount = news.Count;
            Assert.AreEqual(newsCount, articlesView.Items.Count);

            //click the particular symbol of the Position table
            symbolRow.Click();

            //Get the handler of the News Article View displayed
            articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));

            Assert.AreEqual(newsCount, articlesView.Items.Count);
        }

        /// <summary>
        /// On click of a particular symbol in the position table, the news articles corresponding to the 
        /// selected symbol should be displayed.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get a particular symbol from the position table
        /// 3. Click on the symbol row.
        /// 4. Check the news articles are displayed corresponding to the selected symbol.
        /// 5. Click on the symbol2 row.
        /// 6. Check the news articles are displayed corresponding to the newly selected symbol.
        /// 
        /// Expected Result:
        /// Particular Symbol click in the position table should display news articles corresponding
        /// to the selected symbol.
        /// </summary>
        [TestMethod]
        public void ClickingSymbolDisplaysNewsArticlesCorrespondingly()
        {            
            //get handle of the Position Table 
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            string symbol;
            List<News> news;
            ListBox articlesView;

            //click the particular symbol of the Position table
            ListViewRow symbolRow = list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("Symbol")));
            symbolRow.Click();

            //Get the news item for a particular symbol
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(symbol);

            //Get the handler of the News Article View displayed
            articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));

            Assert.AreEqual(news.Count,articlesView.Items.Count);

            //click on the position symbol STOCK6
            symbolRow = list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("PositionSymbol")));
            symbolRow.Click();

            //Get the news item for a particular symbol
            symbol = TestDataInfrastructure.GetTestInputData("PositionSymbol");
            news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(symbol);

            //Get the handler of the News Article View displayed
            articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));

            Assert.AreEqual(news.Count, articlesView.Items.Count);

        }

        /// <summary>
        /// Repeated clicks on already shown symbol in the position table, should display the  
        /// news articles for the recently selected symbol.
        /// 
        /// Repro Steps:
        /// 1. Launch the StockTraderRI
        /// 2. Get a particular symbol row in the position table.
        /// 3. Click on the symbols row STOCK0.
        /// 4. Click on a different symbol in the position table.
        /// 5. CLick on the symbol row STOCK0 again.
        /// 6. The news articles should be displayed corresponding to the recenlty selected symbol.
        /// 
        /// Expected Result:
        /// Repeated clicks on already shown symbol in the position table, should display the  
        /// news articles for the recently selected symbol.
        /// </summary>
        [TestMethod]
        public void ClickingAlreadyShownSymbolDisplaysNewsArticlesCorrespondingly()
        {
            //get handle of the Position Table 
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));
            string symbol;
            List<News> news;
            ListBox articlesView;

            //click the particular symbol of the Position table STOCK0
            ListViewRow symbolRow = list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("Symbol")));
            symbolRow.Click();

            //click on the position symbol STOCK6
            ListViewRow secondSymbolRow  = list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("PositionSymbol")));
            secondSymbolRow.Click();

            //click the particular symbol of the Position table STOCK0            
            symbolRow.Click();

            //Get the news item for a particular symbol
            symbol = TestDataInfrastructure.GetTestInputData("Symbol");
            news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(symbol);

            //Get the handler of the News Article View displayed
            articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));

            Assert.AreEqual(news.Count, articlesView.Items.Count);
        }

        /// <summary>
        /// Check if the News Reader window of the selected Symbol displays news message from the News Data XML file
        /// 
        /// Repro steps:
        /// 1. Launch the StockTrader RI application
        /// 2. Click on the particular position row for a symbol
        /// 3. Check if news article(s) are displayed for the particular symbol
        /// 4. Click on a news article item.
        /// 5. Check if the News Reader window of the selected Symbol displays news message from the News Data XML file
        /// 
        /// Expected Result:
        /// The news reader window is displayed, with the corresponding News text display.
        /// </summary>
        [TestMethod]
        public void DisplayedNewsDoubleClick()
        {
            //Get the handle of the Position table
            ListView list = Window.Get<ListView>(TestDataInfrastructure.GetControlId("PositionTableId"));

            //Click on the particular symbol row
            list.Rows.Find(row => row.Cells[TestDataInfrastructure.GetTestInputData("PositionTableSymbol")].Text.Equals(TestDataInfrastructure.GetTestInputData("Symbol"))).Click();

            //Get the news item for the particular symbol row
            List<News> news = TestDataInfrastructure.GetDataForId<NewsDataProvider, News>(TestDataInfrastructure.GetTestInputData("Symbol"));

            //Get the handle of the articles view
            ListBox articlesView = Window.Get<ListBox>(TestDataInfrastructure.GetControlId("NewsArticlesView"));
            
            //Double click the first news article item
            articlesView.Items[0].DoubleClick();

            //Get the handle of the News Reader window
            Window newsWindow = App.GetWindows().Find(newsArticleWindow => newsArticleWindow.Title.Equals("News Reader"));
            Assert.IsNotNull(newsWindow);

            //Geth the Body,Title and Published Date from the News Reader window
            string body = newsWindow.Get<TextBox>(TestDataInfrastructure.GetControlId("ArticleBody")).Text;
            string title = newsWindow.Get<Label>(TestDataInfrastructure.GetControlId("ArticleTitle")).Text;
            DateTime publishedDate = Convert.ToDateTime(newsWindow.Get<Label>(TestDataInfrastructure.GetControlId("ArticlePublishedDate")).Text, CultureInfo.CurrentCulture);

            Assert.AreEqual(body,news[0].Body);
            Assert.AreEqual(title, news[0].Title);
            Assert.AreEqual(publishedDate, news[0].PublishedDate);           
        }
    }
}

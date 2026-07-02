using PDS.ViewYourFunding.Services.Attributes;
using System.ComponentModel;

namespace PDS.ViewYourFunding.Services.Enums
{
    /// <summary>
    /// All component types.
    /// </summary>
    public enum ComponentType
    {
        /// <summary>
        /// Catch if not set.
        /// </summary>
        NotSet,

        /// <summary>
        /// Design components - download a document (used internally - use Block_DownloadADocument in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/DownloadADocument")]
        DesignComponents_DownloadADocument,

        /// <summary>
        /// Design components - download a document (used internally - use Block_DownloadADocument in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/SectionDownloadADocument")]
        DesignComponents_SectionDownloadADocument,

        /// <summary>
        /// Design components - print or save a statement(used internally - use Block_PrintOrSaveStatement in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/PrintOrSaveStatement")]
        DesignComponents_PrintOrSaveStatement,

        /// <summary>
        /// Design components - warning text(used internally - use Block_WarningText in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/WarningText")]
        DesignComponents_WarningText,

        /// <summary>
        /// Design components - external link (used internally - use General_ExternalLink in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/ExternalLink")]
        DesignComponents_ExternalLink,

        /// <summary>
        /// Design components - funding header (used internally - use  in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/FundingHeader")]
        DesignComponents_FundingHeader,

        /// <summary>
        /// Design components - funding table (used internally - use  in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/FundingTable")]
        DesignComponents_FundingTable,

        /// <summary>
        /// Design components - funding totals (used internally - use  in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/FundingTotals")]
        DesignComponents_FundingTotals,

        /// <summary>
        /// A variance section (renders the variance components).
        /// </summary>
        [DefaultValue("_DesignComponents/Variance")]
        DesignComponents_Variance,

        /// <summary>
        /// Design components - links to page with tabs (used internally - use  in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/LinksToPageWithTabs")]
        DesignComponents_LinksToPageWithTabs,

        /// <summary>
        /// Design components - link with action (used internally - use  in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/LinkWithAction")]
        DesignComponents_LinkWithAction,

        /// <summary>
        /// Design components - standard link control (used internally - use Block_DownloadADocument in JSON instead).
        /// </summary>
        [DefaultValue("_DesignComponents/StandardLink")]
        DesignComponents_StandardLink,

        /// <summary>
        /// A button span section for an accordion (only used internaly).
        /// </summary>
        [DefaultValue("Accordion/_ButtonSpan")]
        Accordion_ButtonSpan,

        /// <summary>
        /// A MAT button span section for an accordion (only used internaly).
        /// </summary>
        [DefaultValue("Accordion/_MATButtonSpan")]
        Accordion_MATButtonSpan,

        /// <summary>
        /// An accordion right aligned section (only used internaly).
        /// </summary>
        [DefaultValue("Accordion/_RightSection")]
        Accordion_RightSection,

        /// <summary>
        /// An accordion MAT right aligned section (only used internaly).
        /// </summary>
        [DefaultValue("Accordion/_MATRightSection")]
        Accordion_MATRightSection,

        /// <summary>
        /// An accordion title for a period  (only used internaly)..
        /// </summary>
        [DefaultValue("Accordion/_TitleForPeriod")]
        Accordion_TitleForPeriod,

        /// <summary>
        /// An accordion title for a period  (only used internaly)..
        /// </summary>
        [DefaultValue("Accordion/_TitleForPeriodLoggedIn")]
        Accordion_TitleForPeriodLoggedIn,

        /// <summary>
        /// An MAT accordion title for a period  (only used internaly)..
        /// </summary>
        [DefaultValue("Accordion/_MATTitleForPeriodLoggedIn")]
        Accordion_MATTitleForPeriodLoggedIn,

        /// <summary>
        /// An accordion open button.
        /// </summary>
        Accordion_Button,

        /// <summary>
        /// IYO accordion button
        /// </summary>
        Accordion_IYOButton,

        /// <summary>
        /// An acordion panel.
        /// </summary>
        Accordion_Panel,

        /// <summary>
        /// An accordion subtitle.
        /// </summary>
        Accordion_SubTitle,

        /// <summary>
        /// An accordion MAT subtitle.
        /// </summary>
        Accordion_MATSubTitle,

        /// <summary>
        /// And accordion title.
        /// </summary>
        Accordion_Title,

        /// <summary>
        /// And accordion MAT title.
        /// </summary>
        Accordion_MATTitle,

        /// <summary>
        /// An accordion total.
        /// </summary>
        [Defaults(NumberFormat = "C0")]
        Accordion_Total,

        /// <summary>
        /// An accordion total.
        /// </summary>
        [Defaults(NumberFormat = "C0")]
        Accordion_MATTotal,

        /// <summary>
        /// An allocation history block.
        /// </summary>
        Block_AllocationHistory,

        /// <summary>
        /// An la recoupement allocation history block.
        /// </summary>
        Block_LARecoupementAllocationHistory,

        /// <summary>
        /// An allocation history block for a single year.
        /// </summary>
        Block_AllocationHistorySingleYear,

        /// <summary>
        /// A download a document block.
        /// </summary>
        Block_DownloadADocument,

        /// <summary>
        /// A download a document block in a section.
        /// </summary>
        Block_SectionDownloadADocument,

        /// <summary>
        /// A print or save statement block.
        /// </summary>
        Block_PrintOrSaveStatement,

        /// <summary>
        /// A warning text block.
        /// </summary>
        Block_WarningText,

        /// <summary>
        /// A partial pupil numbers block.
        /// </summary>
        Block_PartialPupilNumbers,

        /// <summary>
        /// The payment dates block.
        /// </summary>
        Block_PaymentDates,

        /// <summary>
        /// The LA recoupment history block.
        /// </summary>
        Block_RecoupmentHistory,

        /// <summary>
        /// A conditional component.
        /// </summary>
        Conditional_Condition,

        /// <summary>
        /// The 'false' output of a conditional component.
        /// </summary>
        Conditional_FalseOutput,

        /// <summary>
        /// The 'true' output of a conditional component.
        /// </summary>
        Conditional_TrueOutput,

        /// <summary>
        /// A conditional switch component.
        /// </summary>
        Conditional_Switch,

        /// <summary>
        /// A size component.
        /// </summary>
        Data_Size,

        /// <summary>
        /// A value component.
        /// </summary>
        Data_Value,

        /// <summary>
        /// An error component (used internally).
        /// </summary>
        [DefaultValue("Error/_Error")]
        [ErrorType]
        Error_Error,

        /// <summary>
        /// An error component for when a component isn't found by name.
        /// </summary>
        [ErrorType]
        Error_ComponentNotFound,

        /// <summary>
        /// An error component for an exception.
        /// </summary>
        [ErrorType]
        Error_Exception,

        /// <summary>
        /// A model not found component.
        /// </summary>
        [ErrorType]
        Error_ModelNotFound,

        /// <summary>
        /// A model not set component.
        /// </summary>
        [ErrorType]
        Error_ModelNotSet,

        /// <summary>
        /// A model not found component.
        /// </summary>
        [ErrorType]
        Error_ModelError,

        /// <summary>
        /// An error component (used internally) to be shown on live.
        /// </summary>
        [DefaultValue("Error/_ReleaseError")]
        [ErrorType]
        Error_ReleaseError,

        /// <summary>
        /// An error showing something should not display.
        /// </summary>
        [ErrorType]
        Error_ShouldNotDisplay,

        /// <summary>
        /// A GDS detail disclosure component.
        /// </summary>
        GDS_Detail,

        /// <summary>
        /// The back to top link
        /// </summary>
        General_BackToTopLink,

        /// <summary>
        /// A bordered panel.
        /// </summary>
        General_BorderedPanel,

        /// <summary>
        /// An external link.
        /// </summary>
        General_ExternalLink,

        /// <summary>
        /// A literal control (like some text or html).
        /// </summary>
        General_Literal,

        /// <summary>
        /// An unprocessed literal control (no replacements are made on it).
        /// </summary>
        General_RawLiteral,

        /// <summary>
        /// A 'print this page' component.
        /// </summary>
        General_PrintThisPage,

        /// <summary>
        /// A published date ('Published date: [publisheddate]'.
        /// </summary>
        General_PublishedDate,

        /// <summary>
        /// Provider name with Urn.
        /// </summary>
        General_ProviderNameWithUrn,

        /// <summary>
        /// Provider name with LA,Urn.
        /// </summary>
        General_ProviderNameWithUrnLA,

        /// <summary>
        /// A repeater.
        /// </summary>
        General_Repeater,

        /// <summary>
        /// A section (renders the child components).
        /// </summary>
        General_Section,

        /// <summary>
        /// A standard link.
        /// </summary>
        General_StandardLink,

        /// <summary>
        /// A funding header.
        /// </summary>
        [Defaults(NumberFormat = "C0")]
        Heading_FundingHeader,

        /// <summary>
        /// A large sized heading.
        /// </summary>
        Heading_Large,

        /// <summary>
        /// A medium sized heading.
        /// </summary>
        Heading_Medium,

        /// <summary>
        /// A small heading.
        /// </summary>
        Heading_Small,

        /// <summary>
        /// A title heading with a date.
        /// </summary>
        Heading_TitleWithDate,

        /// <summary>
        /// A title with the funding stream name.
        /// </summary>
        Heading_TitleWithFundingStream,

        /// <summary>
        /// A title with the dates for logged in view.
        /// </summary>
        Heading_TitleWithDatesLoggedIn,

        /// <summary>
        /// An HTML Div element.
        /// </summary>
        Html_Div,

        /// <summary>
        /// An HTML Strong element.
        /// </summary>
        Html_Strong,

        /// <summary>
        /// An HTML div with page break CSS.
        /// </summary>
        Html_PageBreak,

        /// <summary>
        /// An HTML Section element.
        /// </summary>
        Html_Section,

        /// <summary>
        /// An HTML elemnt (sub type controls the tag).
        /// </summary>
        Html_Element,

        /// <summary>
        /// A HTML P element.
        /// </summary>
        Html_Paragraph,

        /// <summary>
        /// An HTML script element.
        /// </summary>
        Html_Script,

        /// <summary>
        /// An HTML span element.
        /// </summary>
        Html_Span,

        /// <summary>
        /// A change arrow (up or down pointing arrow).
        /// </summary>
        Pattern_ChangeArrow,

        /// <summary>
        /// An unordered list.
        /// </summary>
        Pattern_UnorderedList,

        /// <summary>
        /// A table column.
        /// </summary>
        Table_Column,

        /// <summary>
        /// A table column row..
        /// </summary>
        Table_ColumnRow,

        /// <summary>
        /// A collection of table columns.
        /// </summary>
        Table_Columns,

        /// <summary>
        /// A table row.
        /// </summary>
        Table_Row,

        /// <summary>
        /// A table footer.
        /// </summary>
        Table_Footer,

        /// <summary>
        /// A table rows column.
        /// </summary>
        Table_RowColumn,

        /// <summary>
        /// A collection of table rows.
        /// </summary>
        Table_Rows,

        /// <summary>
        /// A table.
        /// </summary>
        Table_Table,

        /// <summary>
        /// Links to tab pages.
        /// </summary>
        Tabs_LinksToPage,

        /// <summary>
        /// A link with a monetry value after it.
        /// </summary>
        Tabs_LinkWithValue,

        /// <summary>
        /// A tab.
        /// </summary>
        Tabs_Tab,

        /// <summary>
        /// A set of tabs.
        /// </summary>
        Tabs_Tabs,

        /// <summary>
        /// A 'final' tag.
        /// </summary>
        Tag_Final,

        /// <summary>
        /// A 'final' tag (or nothing).
        /// </summary>
        Tag_FinalOrNothing,

        /// <summary>
        /// A 'latest' tag.
        /// </summary>
        Tag_Latest,

        /// <summary>
        /// Renders a 'Latest' or 'Final' tag (or nothing).
        /// </summary>
        Tag_LatestFinalOrNothing,

        /// <summary>
        /// Renders a 'Latest' 'Final' or Not latest tag.
        /// </summary>
        Tag_LatestFinalOrNotLatest,

        /// <summary>
        /// Renders a 'Latest' or 'Not latest' tag.
        /// </summary>
        Tag_LatestNotLatest,

        /// <summary>
        /// A 'latest' tag (or nothing).
        /// </summary>
        Tag_LatestOrNothing,

        /// <summary>
        /// Renders a 'Not latest' tag.
        /// </summary>
        Tag_NotLatest,

        /// <summary>
        /// Renders a tag.
        /// </summary>
        Tag_Tag,

        /// <summary>
        /// A grand total component.
        /// </summary>
        Total_GrandTotal,

        /// <summary>
        /// A sub total component.
        /// </summary>
        Total_SubTotal,

        /// <summary>
        /// A plus (+) symbol component.
        /// </summary>
        Symbols_Plus,

        /// <summary>
        /// A minus (-) symbol component.
        /// </summary>
        Symbols_Minus,

        /// <summary>
        /// An equals (=) symbol component.
        /// </summary>
        Symbols_Equals,

        /// <summary>
        /// A divide (÷) symbol component.
        /// </summary>
        Symbols_Divide,

        /// <summary>
        /// A times (x) symbol component.
        /// </summary>
        Symbols_Times,

        /// <summary>
        /// A days in year component (displays the number of days for the current year).
        /// </summary>
        GDS_DaysInYear,

        /// <summary>
        /// A download document component for logged in views.
        /// </summary>
        Block_LoggedInDownloadADocument,

        /// <summary>
        /// A variance message display component for views.
        /// </summary>
        Block_VarianceMessage,

        /// <summary>
        /// The block comparison view
        /// </summary>
        Block_ComparisonView,

        /// <summary>
        /// An alternative title for the component to be rendered.
        /// </summary>
        Heading_AlternativeTitle,

        /// <summary>
        /// A title heading with only date.
        /// </summary>
        Heading_TitleWithOnlyDate,

        /// <summary>
        /// The date formatted in dd/MM/yyyy.
        /// </summary>
        Block_DateFormattedddmmyyyy,

        /// <summary>
        /// A download document component for logged in views for funding stream LAREC.
        /// </summary>
        Block_LoggedInDownloadADocumentLAREC
    }
}
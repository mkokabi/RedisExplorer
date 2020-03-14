using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace RedEx
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("a007df23-0e76-4cb9-abf2-2c1c92ec839b")]
    public class RedExToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedExToolWindow"/> class.
        /// </summary>
        public RedExToolWindow() : base(null)
        {
            this.Caption = "RedExToolWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new RedExToolWindowControl();
        }
    }
}

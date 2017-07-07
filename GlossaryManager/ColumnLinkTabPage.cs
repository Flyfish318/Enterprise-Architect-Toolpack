using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using EAWrapped=TSF.UmlToolingFramework.Wrappers.EA;

namespace GlossaryManager {

	public class ColumnLinkTabPage : FocusableTabPage {

    private GlossaryManagerUI ui;
    private ToolStripLabel notificationLabel;
    private TreeView tree;
    
    public ColumnLinkTabPage(GlossaryManagerUI ui) : base() {
      this.ui   = ui;
      this.Text = "Column Links";
      
      this.addToolbar();

      this.tree = new TreeView() {
        Dock = DockStyle.Fill
      };
      this.Controls.Add(tree);
      
      this.ui.NewContext += new NewContextHandler(this.handleContextChange);
    }
    
    private void handleContextChange(EAWrapped.ElementWrapper context) {
      if(context == null) { return; }
      this.checkContext(context);
    }

    public override void HandleFocus() {
      this.checkContext();
    }

    private void checkContext() {
      this.checkContext(this.ui.Addin.SelectedItem);
    }

    private void checkContext(EAWrapped.ElementWrapper context) {
      this.notify("Please select a Data Item or Table");

      this.tree.Nodes.Clear();

      // detect context and dispatch to approriate UI builder
      if(context == null) { return; }

      if( ! (context is EAWrapped.Class) ) { return; }

      EAWrapped.Class clazz = context as EAWrapped.Class;
      if( clazz.stereotypes.Count != 1 ) { return; }

      switch(clazz.stereotypes.ToList()[0].name) {
        case "Data Item":  this.showDataItem(clazz); break;
        case "table":      this.showTable(clazz);    break;
        default: return;
      }

      this.hideNotifications();
    }

    private void showDataItem(EAWrapped.Class item) {
      this.tree.Nodes.Add(new TreeNode(item.name));
      // TODO find columns that link to this
      // TODO add tables and columns
    }

    private void showTable(EAWrapped.Class table) {
      this.tree.Nodes.Add(new TreeNode(table.name));      
      // TODO loop over columns and add dataitems that are linked
    }

    private void addToolbar() {
      ToolStrip toolStrip = new ToolStrip() {
        Dock = DockStyle.Bottom
      };

      this.notificationLabel = new ToolStripLabel();
      toolStrip.Items.Add(this.notificationLabel); 

      this.Controls.Add(toolStrip);
    }

    private void notify(string msg) {
      this.notificationLabel.Text = msg;
      this.notificationLabel.Visible = true;
    }

    private void hideNotifications() {
      this.notificationLabel.Visible = false;
    }

  }
}

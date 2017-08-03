using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntiAutoAwakeWatcher
{
    enum WpfNotifyIconActionType { Item, Seperator }
    class WpfNotifyIconAction
    {
        public string Text { get; set; }
        public Action Action { get; set; }
        public bool? IsChecked { get; set; }
        public Action<bool> IsCheckedChanged { get; set; }
        public WpfNotifyIconActionType Type { get; set; }
    }

    class WpfNotifyIcon : IDisposable
    {
        private readonly NotifyIcon notifyIcon;
        private readonly ContextMenuStrip contextMenuStripNotfiyIcon;
        private readonly ToolStripItem[] toolStripMenuItems;
        
        public bool IsVisible
        {
            get { return notifyIcon.Visible; }
            set { notifyIcon.Visible = true; }
        }

        public WpfNotifyIcon(string notfiyIconText, IEnumerable<WpfNotifyIconAction> actions)
        {
            notifyIcon = new NotifyIcon();
            contextMenuStripNotfiyIcon = new ContextMenuStrip();
            contextMenuStripNotfiyIcon.SuspendLayout();

            // 
            // notifyIcon
            // 
            notifyIcon.Icon = Properties.Resources.defaultIcon;
            notifyIcon.Text = "AntiAutoAwake Watcher";
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = contextMenuStripNotfiyIcon;
            
            // 
            // contextMenuStripNotfiyIcon
            // 
            
            contextMenuStripNotfiyIcon.Items.AddRange(toolStripMenuItems = CreateItems(actions).ToArray());
            contextMenuStripNotfiyIcon.Name = "contextMenuStripNotfiyIcon";
            contextMenuStripNotfiyIcon.RenderMode = ToolStripRenderMode.System;
            //contextMenuStripNotfiyIcon.Size = new System.Drawing.Size(177, 26);

            contextMenuStripNotfiyIcon.ResumeLayout(false);
        }

        private IEnumerable<ToolStripItem> CreateItems(IEnumerable<WpfNotifyIconAction> actions)
        {
            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();
            foreach (var action in actions)
            {
                ToolStripItem item;
                if (action.Type == WpfNotifyIconActionType.Item)
                {
                    var menuItem = new ToolStripMenuItem();
                    menuItem.Text = action.Text;
                    if (action.Action != null)
                        menuItem.Click += (_, __) => action?.Action();

                    menuItem.Checked = action.IsChecked ?? false;
                    if (action.IsCheckedChanged != null)
                    {
                        menuItem.CheckedChanged += (_, __) => action.IsCheckedChanged(menuItem.Checked);
                        menuItem.Click += (_, __) => menuItem.Checked = !menuItem.Checked;
                    }
                    item = menuItem;
                }
                else if (action.Type == WpfNotifyIconActionType.Seperator)
                {
                    item = new ToolStripSeparator();
                }
                else throw new NotImplementedException();


                yield return item;
            }
        }

        void CreateNotify()
        {
            
            // 
            // deactivateWatcherToolStripMenuItem
            // 
            //deactivateWatcherToolStripMenuItem.Name = "deactivateWatcherToolStripMenuItem";
            //deactivateWatcherToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            //deactivateWatcherToolStripMenuItem.Text = "Deactivate Watcher";
            //deactivateWatcherToolStripMenuItem.Click += DeactivateWatcher_Clicked;


        }

        public void Dispose()
        {
            notifyIcon.Visible = false;

            contextMenuStripNotfiyIcon.Dispose();
            foreach (var item in toolStripMenuItems)
            {
                item.Dispose();
            }
            notifyIcon.Dispose();
        }
    }
}

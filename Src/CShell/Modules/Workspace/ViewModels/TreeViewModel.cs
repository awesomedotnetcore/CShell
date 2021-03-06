﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace CShell.Modules.Workspace.ViewModels
{
    public class TreeViewModel : PropertyChangedBase, IHaveDisplayName, IDisposable
    {
        private string displayName;
        public virtual string DisplayName
        {
            get { return displayName; }
            set
            {
                //if while a rename ESC is pressed the DisplayName will be set to null 
                if(value == null)
                    return;
                displayName = value;
                NotifyOfPropertyChange(() => DisplayName);
            }
        }

        public virtual Uri IconSource
        {
            get { return new Uri("pack://application:,,,/CShell;component/Resources/Icons/page_white.png"); }
        }

        private readonly IObservableCollection<TreeViewModel> children = new BindableCollection<TreeViewModel>();
        public virtual IObservableCollection<TreeViewModel> Children
        {
            get { return children; }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; ; NotifyOfPropertyChange(() => IsSelected); }
        }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                NotifyOfPropertyChange(() => IsExpanded);
                NotifyOfPropertyChange(() => IconSource);
            }
        }

        private bool isEditable = true;
        public bool IsEditable
        {
            get { return isEditable; }
            set
            {
                isEditable = value;
                NotifyOfPropertyChange(() => IsEditable);
            }
        }

        private bool isInEditMode = false;
        public virtual bool IsInEditMode
        {
            get { return isInEditMode; }
            set
            {
                var previousMode = isInEditMode;
                isInEditMode = value;
                if(previousMode == true && isInEditMode == false)
                    EditModeFinished();
                NotifyOfPropertyChange(() => IsInEditMode);
            }
        }

        protected virtual void EditModeFinished()
        {
            
        }

        /// <summary>
        /// Recursively returns all children in the tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeViewModel> GetAllChildren()
        {
            foreach (var treeViewModel in Children)
            {
                yield return treeViewModel;
                foreach (var subTreeViewModel in treeViewModel.GetAllChildren())
                {
                    yield return subTreeViewModel;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (children != null)
            {
                foreach (var child in Children)
                {
                    child.Dispose(disposing);
                }
            }
        }
    }
}

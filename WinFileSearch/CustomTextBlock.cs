using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WinFileSearch
{
    public class CustomTextBlock : TextBlock
    {
        public InlineCollection InlineCollection
        {
            get
            {
                return (InlineCollection)GetValue(InlineCollectionProperty);
            }
            set
            {
                SetValue(InlineCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty InlineCollectionProperty = DependencyProperty.Register(
            "InlineCollection",
            typeof(InlineCollection),
            typeof(CustomTextBlock),
                new UIPropertyMetadata((PropertyChangedCallback)((sender, args) =>
                {
                    CustomTextBlock textBlock = sender as CustomTextBlock;

                    if (textBlock != null)
                    {
                        textBlock.Inlines.Clear();

                        InlineCollection inlines = args.NewValue as InlineCollection;

                        if (inlines != null)
                            textBlock.Inlines.AddRange(inlines.ToList());
                    }
                })));
    }
}

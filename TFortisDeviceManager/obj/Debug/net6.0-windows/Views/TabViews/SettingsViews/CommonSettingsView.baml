   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      I)clr-namespace:TFortisDeviceManager.ModelsTFortisDeviceManager.Models  5	      3    -TFortisDeviceManager.Views.CommonSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5      6models)clr-namespace:TFortisDeviceManager.Models  5	      .��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��431* �6   .+�5   
   � $��0��6   .+�5      �� $
��Left=�6   $	��564��64   .+�5      �� /   )TFortisDeviceManager.Properties.Resources   CommonSettings#��Z 6   $��	10,30,0,0q�6R   $
��Bold"�6e   $��14(�6w   �� 5      ���	#FFD0D1D7$��0,0,0,1q�62   $��35��6L    J� SnapsToDevicePixels$
 True��6X   $	��531��6s   ._�5      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��0,0,0,0q�6U   .+�5      ]� $	��320��6$   .+�5       ��    KeepEventInDatabase#��Z 6(   $
��Wrap=�6c    �� TextAlignment$
 Left=�6w   $
��Left=�6�   $��Center=�6�   5!      �� 5"      $ 
Horizontal=�6%   $	��140��6>   .+�5#      )  #HandyControl.Controls.NumericUpDown     Minimum$ 90Q�6c      Value 60   �� *KeepEventInDatabase+	 �� Mode$	 TwoWay=�6c   �� 5$      
   Days#��Z
 6(   $��60��6T   $��14(�6_   $��Stretch=�6w   $��Center=�6�   $��5,0,0,0q�6�   5%      5&      5'      �� 5)      ���	#FFD0D1D7$��0,0,0,1q�62   $��35��6L   $
 True��6X   $	��531��6s   ._�5*      �� $ 
Horizontal=�6!   $��Center=�6:   $��0,0,0,0q�6U   .+�5+      ]� $	��320��6$   .+�5,      ��    MapProvider#��Z 6(   $
��Wrap=�6[   $
 Left=�6o   $
��Left=�6�   $��Center=�6�   5-      �� 5.      $ 
Horizontal=�6%   $	��140��6>   .+�5/      $  HandyControl.Controls.ComboBox  $	��120��6*   ��66   �� *ProvidersDict+ �� SelectedValue 6\   �� *SelectedProvider+51      52      53      54      �� 56      ���	#FFD0D1D7$
��10,0q�62   $��0,0,0,1q�6@   $��35��6Z   $
 True��6f   $	��530��6�   ._�57      �� $ 
Horizontal=�6!   $��Center=�6:   .+�58      ]� $	��320��6$   .+�59      ��    AppLanguage#��Z 6(   $
��Wrap=�6[   $
 Left=�6o   $
��Left=�6�   $��Center=�6�   5:      �� 5;      $ 
Horizontal=�6%   $	��140��6>   .+�5<        -   5          Name$ ComboBoxSelectLanguage��5=   *   $	��120��5<   *    �� DisplayMemberPath$ Value��5?   *    �� SelectedValuePath$	 Key��5@   *   ��5>   *   �� *LanguageDict+$	 OneWay=� 5A   *   �� *AppLanguage+$	 TwoWay=�5C      5D      5E      �� 5G      ���	#FFD0D1D7$
��10,0q�62   $��0,0,0,1q�6@   $��35��6Z   $
 True��6f   $	��530��6�   ._�5H      �� $ 
Horizontal=�6!   $��Center=�6:   .+�5I      ]� $	��320��6$   .+�5J      ��    Theme#��Z 6(   $
��Wrap=�6U   $
 Left=�6i   $
��Left=�6~   $��Center=�6�   5K      �� 5L      $ 
Horizontal=�6%   $	��140��6>   .+�5M        -   5        $ ComboBoxSelectTheme��5N      $	��120��5M   *   $ Value��5P      $	 Key��5Q      ��5O      �� *	ThemeDict+$	 OneWay=� 5R      �� *
AppTheme+$	 TwoWay=� �� SelectionChanged 5S      !  Stylet.Xaml.ActionExtension  *ChangeThemeCommand+5T      5U      5V      5W      5X      5Z      5[      
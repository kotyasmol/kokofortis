   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      I)clr-namespace:TFortisDeviceManager.ModelsTFortisDeviceManager.Models  6O   3    -TFortisDeviceManager.Views.SearchSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5      6models)clr-namespace:TFortisDeviceManager.Models  6O   .��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��431* �6   .+�5   
   � $��0��6   .+�5      �� $
��Left=�6   $	��564��64   .+�5      �� /   )TFortisDeviceManager.Properties.Resources   SearchSettings#��Z 6   $��	10,30,0,0q�6R   $
��Bold"�6e   $��14(�6w   �� 5      ���	#FFD0D1D7$��0,0,0,1q�62   $��35��6L    J� SnapsToDevicePixels$
 True��6X   $	��531��6s   ._�5      ��  �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��0,0,0,0q�6U   .+�5      ]� $	��320��6$   .+�5      ��    SearchTimeout#��Z 6(   $
��Wrap=�6]    �� TextAlignment$
 Left=�6q   $
��Left=�6�   $��Center=�6�   5      �� 5       $ 
Horizontal=�6%   $	��140��6>   .+�5!      )  #HandyControl.Controls.NumericUpDown     Maximum$	 120Q�5#   !      Minimum$ 10Q�5$   !   	   Value	 5"   !   �� *SearchTimeout+
 �� Mode$
 TwoWay=�5$   !   �� 5%         Seconds#��Z 6(   $��60��6W   $��14(�6b   $��Stretch=�6z   $��Center=�6�   $��5,0,0,0q�6�   5&      5'      5(      5)      5*      5-      5.      
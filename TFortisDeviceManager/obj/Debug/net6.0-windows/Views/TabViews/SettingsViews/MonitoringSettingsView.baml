   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5	      I)clr-namespace:TFortisDeviceManager.ModelsTFortisDeviceManager.Models  6O   7    1TFortisDeviceManager.Views.MonitoringSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      9local-clr-namespace:TFortisDeviceManager.Converters  5      >
properties-clr-namespace:TFortisDeviceManager.Properties  5	      6models)clr-namespace:TFortisDeviceManager.Models  6O     � 	Resources  5      =   7TFortisDeviceManager.Converters.ColumnSettingsConverter   ColumnSettingsConverter��%   &	          5   
   6(   5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6   $��60��6*   .+�5   
   �� $
��Left=�6   $	��564��60   .+�5      �� /   )TFortisDeviceManager.Properties.Resources   MonitoringSettings#��Z 6   $��	10,30,0,0q�6R   $
��Bold"�6e   $��14(�6w   �� 5      ���	#FFD0D1D7$��	10,0,10,0q�6.   $��0,0,0,1q�6A   ._�5      ��  �� Orientation$ 
Horizontal=�6   $��
10,10,0,10q�66   .+�5      ]� $	��320��6    .+�5       ��    	SNMPAwait#��Z 6$   $
��Wrap=�6U    �� TextAlignment$
 Left=�6i   $
��Left=�6~   $��Center=�6�   5!      �� 5"      $ 
Horizontal=�6!   $	��140��6:   .+�5#      )  #HandyControl.Controls.NumericUpDown     Minimum$ 5Q�6W      Value 6,   �� *SNMPTimeout+ �� Mode$ TwoWay=�6W   �� 5$      	   Seconds#��Z	 6$   $��60��6S   $��Center=�6^   $��5,0,0,0q�6y   5%      5&      5'      �� 5)      ���	#FFD0D1D7$��	10,0,10,0q�6-   $��0,0,0,1q�6@   ._�5*      �� $ 
Horizontal=�6   $��Center=�66   $��
10,10,0,10q�6Q   .+�5+      ]� $	��320��6    .+�5,      �� 
   UptimeCheckPeriod#��Z
 6$   $
��Wrap=�6]   $
 Left=�6q   $
��Left=�6�   $��Center=�6�   5-      �� 5.      $ 
Horizontal=�6!   $	��140��6:   .+�5/        $ 10Q�6]    6,   �� *UptimeCheckPeriod+$ TwoWay=�6]   �� 50      #��Z	 6$   $��60��6S   $��Center=�6^   $��5,0,0,0q�6y   51      52      53      �� 55      ���	#FFD0D1D7$��	10,0,10,0q�6.   $��0,0,0,1q�6A   ._�56      �� $ 
Horizontal=�6   $��Center=�66   $��
10,10,0,10q�6Q   .+�58      ]� $	��320��6    .+�59      ��    SensorReadPeriod#��Z 6$   $
��Wrap=�6\   $
 Left=�6p   $
��Left=�6�   $��Center=�6�   5:      �� 5<      $ 
Horizontal=�6!   $	��140��6:   .+�5=        $ 10Q�6\    6,   �� *SensorReadPeriod+$ TwoWay=�6\   �� 5>      #��Z	 6$   $��60��6S   $��Center=�6^   $��5,0,0,0q�6y   5?      5@      5A      �� 5C      ���	#FFD0D1D7$��	10,0,10,0q�6.   $��0,0,0,1q�6A   ._�5D      �� $ 
Horizontal=�6   $��Center=�66   $��
10,10,0,10q�6Q   .+�5F      ]� $	��320��6    .+�5G      ��    AutoHideNotifications#��Z 6$   $
��Wrap=�6a   $
 Left=�6u   $
��Left=�6�   $��Center=�6�   5H      �� 5J      $ 
Horizontal=�6!   $	��140��6:   .+�5L      �� $��Right=�6#    d� 	IsChecked 6?   �� *IsAutoHideNotifications+5M      5N      5O      �� 5Q      ���	#FFD0D1D7$��	10,0,10,0q�6.   ._�5R      �� $ 
Horizontal=�6   $��Center=�66   $��
10,10,0,10q�6Q   .+�5T      ]� $	��320��6    .+�5U      ��    HideNotificationsAfter#��Z 6$   $
��Wrap=�6b   $
 Left=�6v   $
��Left=�6�   $��Center=�6�   5V      �� 5X      $ 
Horizontal=�6!   $	��140��6:   .+�5Y        $ 10Q�6b      Maximum$ 60Q�6o    6,   �� *HideNotificationsAfter+$ TwoWay=�{�6|   �� *IsAutoHideNotifications+�� 5Z      #��Z	 6$   $��60��6S   $��Center=�6^   $��5,0,0,0q�6y   5[      5\      5]      �� 5_         ColumnVisibilitySettings#��Z 6   $��	10,30,0,0q�6X   $
��Bold"�6k   $��14(�6}   �� 5a      ���	#FFD0D1D7$��	10,0,10,0q�6"   ._�5b      �� $ 
Horizontal=�6   $��Center=�66   $��Center=�6Q   $��
10,10,0,10q�6n   .+�5c      )  #HandyControl.Controls.CheckComboBox  -   5          Name$ DeviceColumnsList��5c   (   $��0,0,50,0q�5d   (   $	��200��5g   (   (  "HandyControl.Controls.TitleElement   
TitleWidth$	 140 �5h   (      TitlePlacement$	 Top=�5i   (      ChooseColumns'  !HandyControl.Controls.InfoElement   Placeholder# Z 5j   (      DevicesTable   Title# Z 5k   (     CheckComboBoxPlus#��[ 5l   (   )  #HandyControl.Controls.ListBoxAttach   SelectedItems 5e   (   �� *SelectedDeviceColumns+$ OneWay=���5f   (   �� *DeviceColumnsVisibility+$ OneWay=�5m        5o      -   5        $ EventColumnsList��5o   '   $��50,0,0,0q�5p   '   $	��380��5q   '   $	��200��5t   '   $	 140 �63   $	 Top=�5u   '   # Z 5v   '      EventsTable# Z 5w   '   #��[ 5x   '    5r   '   �� *SelectedEventColumns+$ OneWay=���5s   '   �� *EventColumnsVisibility+5x   '   5y      5z      �� 5{      $ 
Horizontal=�6   $��
10,10,0,10q�62   .+�5}      ��   ButtonPrimary#��[ 5~         Confirm#��Z 5      $��	20,0,40,0q�5�      $
��Left=�5�       � Cursor$
 Hand��5�      ��5�      !  Stylet.Xaml.ActionExtension  *SetDeviceTableSettings+��5�      `� 5�       `� 	Converter# [  6'   .1�5�      ��  �� ElementName$ DeviceColumnsList��6&    �� Path$ SelectedItems�6F   �� 5�      $ EventColumnsList��6&   $ SelectedItems�6E   5�      5�      �� 5�      #��[ 6      EventExport#��Z 5�   	   $��20,0,0,0q�5�   	   $
��Left=�5�   	   $
 Hand��5�   	   ��5�   	     *ExportEventsCommand+5�      5�      5�      5�      5�      
   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      G(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5	      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5
      1    +TFortisDeviceManager.Views.SntpSettingsView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ?local-clr-namespace:TFortisDeviceManager.Converters     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      4views(clr-namespace:TFortisDeviceManager.Views  5	      >
properties-clr-namespace:TFortisDeviceManager.Properties  5
      A =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet  5        � 	Resources  5      >   8TFortisDeviceManager.Converters.ObjectToBooleanConverter   ObjectToBooleanConverter�� 5   
   %   &	          5      6-   5      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6   �� 5      $
��Auto �6   $��70��6*   �� 5      $��40 �6   .+�5   
   �� {�6   ��  �� ElementName$ 
DeviceList�� �� Path$ SelectedItem� �� 	Converter# [   �� Mode$ TwoWay=�.+�5      �� $��	20,0,20,0q�6    �� Orientation$ Vertical=�5       $��Center=�5!      .+�5#      ��   BorderBrush#���  6   $��40��5$      $��0,0,0,1q�5%      ._�5'      �� $ 
Horizontal=�6!   $��5q�5(   !   .+�5*      �� $	��150��6$   /   )TFortisDeviceManager.Properties.Resources   ServerIpAddress#��Z 5+   $   $��Center=�5,   $   #  HandyControl.Controls.TextBox  5.      $	��200��6%    �� TextAlignment$ Center=�5/   %   	 � Cursor$	 IBeam��51   %   ��50   %   �� *
SntpServer+$ TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=�" �� ValidatesOnNotifyDataErrors$
 True��51   %   52      53      �� 55      #���  6   $��40��56      $��0,0,0,1q�57      ._�59      �� $ 
Horizontal=�6!   $��5q�5:   !   .+�5<      �� $	��150��6$      Timezone#��Z 5=   $   $��Center=�5>   $   $
��Left=�5?   $   $  HandyControl.Controls.ComboBox  5A       �� SelectedValuePath$ Value��5C   &    �� DisplayMemberPath$	 Key��5D   &   $	��130��5E   &   ��5A   &   �� *UTCDictionary+$ OneWay=� �� SelectedValue 5B   &   �� *
Timezone+5E   &   5F      5G      �� 5I      #���  6   $��40��5J      $��0,0,0,1q�5K      ._�5M      �� $ 
Horizontal=�6!   $��5q�5N   !   .+�5P      �� $	��150��6$      Period#��Z 5Q   $   $��Center=�5R   $     5T      $	��130��5U   &    5T   &   �� *
SntpPeriod+@�5W      �� 5X   "   $��1��6/   �� 5Y   "   $��10��6/   �� 5Z   "   $��60��6/   �� 5[   "   $	��240��6/   5^      5_      5`      �� 5b      #���  6   $��40��5c      $��0,0,0,1q�5d      ._�5f      �� $ 
Horizontal=�6!   $��5q�6:   .+�5g      �� $	��150��6$      ConfirmToAll#��Z 5h   $   $
��Wrap=�5i   $    �� TextAlignment$
 Left=�5j   $   $
��Left=�5k   $   $��Center=�5l   $   �� 5n       d� 	IsChecked 6#   �� *	SntpToAll+5o      5p      5r      �� 5t      {�6   �� $ 
DeviceList��$ SelectedItem�# [  $ TwoWay=�.+�5u      �� $��2��6   $��70��5v      $ 
Horizontal=�5w      $��Center=�5x      .+�5z      ��    	UserLogin#��Z 6    $
 Left=�5{       $��40��5|       $��0,0,5,0q�5}       $��Center=�5~         5�      $��30��5�   !   $	��100��5�   !   $	 IBeam��5�   !   ��5�   !   �� *Login+5�   !   /  )HandyControl.Controls.UniformSpacingPanel  5�      $��20��6-   �� 5�         UserPassword#��Z 6    $��0,0,10,0q�6T   $��Center=�6f   '  !HandyControl.Controls.PasswordBox  5�         ShowEyeButton$
 True��5�   %      IsSafeEnabled$ False��5�   %   $��30��5�   %   $	��100��5�   %   $	 IBeam��5�   %      UnsafePassword 5�   %   �� *
Password+5�   %     5�      $��20��6-   *  $HandyControl.Controls.ProgressButton  5�        ToggleButtonLoadingPrimary#��[ 5�   (      Progress$ 0Q�5�   (   $	��100��5�   (      Apply#��Z 5�   (   $��30��5�   (   $
	 Hand��5�   (   ��5�   (   !  Stylet.Xaml.ActionExtension  *ConfirmSntpCommand+ 5�   (   �� *SettingApplyingInProgress+$ TwoWay=�5�   (   5�      �� 5�      $��3��6   $
��10,5q�5�      $��14(�5�      ��5�      �� *ErrorMessage+$ OneWay=���5�      �� *
ErrorColor+5�      5�      5�      5�      5�      
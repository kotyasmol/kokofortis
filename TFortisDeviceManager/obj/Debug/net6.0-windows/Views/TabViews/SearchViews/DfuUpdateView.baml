   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ConvertersTFortisDeviceManager.Converters  5      Q-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      G(clr-namespace:TFortisDeviceManager.ViewsTFortisDeviceManager.Views  5
      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5      .    (TFortisDeviceManager.Views.DfuUpdateView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      ?local-clr-namespace:TFortisDeviceManager.Converters     5      S OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol  5      >
viewmodels-clr-namespace:TFortisDeviceManager.ViewModels  5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag	 cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  	 5	      6views(clr-namespace:TFortisDeviceManager.Views  	 5
      @
properties-clr-namespace:TFortisDeviceManager.Properties  	 5      A
 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 
 5        � 	Resources  5      >   8TFortisDeviceManager.Converters.ObjectToBooleanConverter   ObjectToBooleanConverter�� 5   
   %   &	          5      6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��90��6   $��90 �6+   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�5      $��Center=�5      $	��500��5      {�5       ��  �� ElementName$ 
DeviceList�� �� Path$ SelectedItem� �� 	Converter# [   �� Mode$ TwoWay=�.+�5"      �� $ 
Horizontal=�6   $��Center=�62   $��	0,70,0,70q�6O   .+�5#      �� $	��300��6    �� 
IsReadOnly$
 True��6T   ��6&   �� *SelectedDfuFile+$ TwoWay=�6T   �� 5$      $��0q�6=   
������ $��10,0,0,0q�6�    � Cursor$
 Hand��6�   {�6   �� *CanSelectFile+��6j   ! 
 Stylet.Xaml.ActionExtension  *SelectFileCommand+.��5%      . 	 (MahApps.Metro.IconPacks.PackIconMaterial  	   Kind$	 Folder=�61   5&      �� 5(        ButtonPrimary#��[ 5*      $��10,0,0,0q�5+      $
 Hand��5,      ��5(        *UploadDfuCommand+{�5)      �� *CanStartUpload+.��5.        $	 Upload=�61   50      52      �� 54      $	��100��61   
 �� TextAlignment$
 Center=�6>   $��Center=�6U   $��15(�6r   ��6   �� *	Message+6r   �� 56      $	��200��6   .+�57      -  'HandyControl.Controls.CircleProgressBar  $	��150��6#   $	��150��58   #   $��40(�5;   #   y�59   #   �� *DfuUpdatingInProgress+  Boolean2VisibilityConverter# [  � Value 5:   #   �� *DfuUpdatingProgress+5;   #   5<      5=      �� 5?   
   $	��150��6   $��1��5@      $��30q�5A      $��Bottom=�5C      #��[ 5D      $
 Hand��5E      /   )TFortisDeviceManager.Properties.Resources   Update#��Z 5F      {�5B      �� *CanStartUpdate+��5G        *UpdateDfuCommand+5H      5I      
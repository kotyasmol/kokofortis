   M S B A M L   `   `   `  ����O  KTFortisDeviceManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:TFortisDeviceManager.ViewModelsTFortisDeviceManager.ViewModels  5      Q-clr-namespace:TFortisDeviceManager.PropertiesTFortisDeviceManager.Properties  5	      Y1clr-namespace:TFortisDeviceManager.Infrastructure#TFortisDeviceManager.Infrastructure  5      )    #TFortisDeviceManager.Views.RootView   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      c _MahApps.Metro.IconPacks.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137ag cMahApps.Metro.IconPacks.Material, Version=4.0.0.0, Culture=neutral, PublicKeyToken=0c0d510f9915137a?	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks   5      @
viewModels-clr-namespace:TFortisDeviceManager.ViewModels   5      S	 OHandyControl, Version=3.4.0.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b0hc'https://handyorg.github.io/handycontrol 	 5      >
properties-clr-namespace:TFortisDeviceManager.Properties 	 5	      A
 =Stylet, Version=1.3.6.0, Culture=neutral, PublicKeyToken=null)s!https://github.com/canton7/Stylet 
 5      Finfrastructure1clr-namespace:TFortisDeviceManager.Infrastructure 
 5      -   5        " 	 HandyControl.Controls.Window   Name$
  Root��5       � WindowStartupLocation$ CenterScreen=�5       � WindowState$ 	Maximized=�5         	ShowTitle$
 True��5       J� SnapsToDevicePixels$
 True��5       � Icon$; 5\TFortisDeviceManager;component\Views\Images\icon.ico��5      $
��1000��5      $
��1440��5       � Title 5      �� *Title+   IsFullScreen 5      �� *IsFullScreen+ � 	Resources 5         MainMenuIconStyle	 �� 
TargetType.  (MahApps.Metro.IconPacks.PackIconMaterial
 �� Property  PrimaryTextBrush �� Value  MainMenuIconStyle2  ShowInfoTemplate �� Padding �� ElementName �� Path �� Mode  EmptyTemplate>   8TFortisDeviceManager.Infrastructure.InfoTemplateSelector  InfoTemplateSelector   EmptyTemplate   InfoTemplate�� 5   
   %  &	        &	 �     &	     &	 �    &	 �    7  7  �� 5      "	  6/   .�5      �� 
 � ��# �  6/   �� 5      
 � ��$ 24��6*   �� 5      
 � ��$ 24��6+   5       �� 5!      "	 ��60   .�5"      �� 
 � ��# �  6/   �� 5#      
 � ��$ 24��6*   �� 5$      
 � ��$ 24��6+   5%      �� 5'      .R�5(      �� $ 5q�5)      ��5(      �� $
 Root��$ DataContext.Info�$ OneWay=�5)      5*      �� 5,      5-        5/      8   8  51   .   52      .��55      �  � ColumnDefinitions 56   
   �� 57      ��58      �� 59      "	 ��6   .�5:      �� 
 � ��$	 210 �62    �� Triggers 5;      �� 5<       �� Binding 6*   �� $ MenuSizeButton��$ 	IsChecked� �� Value True6i   .a�5=   "   �� 
 � ��$ 48 �6:   5>      5@      5B      �� 5C      $��* �6    � RowDefinitions 5F   
   �� 5G      $��* �6   �� 5H      $��18 �6   .+�5K   
   & 	  HandyControl.Controls.NotifyIcon  -   5           Text$ TFortisDeviceManager��5K       � ContextMenu 5L      �� 5M      .@�5N      m� -   5        /   )TFortisDeviceManager.Properties.Resources   Expand#��Z 5N   =   m� 5O      -   5           Exit#��Z 5O   <   5P      5R      � 5T   
   $��0��6   $��0��6     RegionBrush#���  5U       5V      �� 5W      $��257.907 �6    �� 5X      $��60.093 �6    �� 5Y      $��* �6    �� 5Z      $
��Auto �6    .+�5]      $ 	 HandyControl.Controls.SideMenu  -   5          Name$ MainSideMenu��5`      $��0��5]         
AutoSelect$
 True��5^      $��2��5`   0   y�5_      �� $ OpenSettingsButton��$ 	IsChecked� ! Boolean2VisibilityReConverter �� 	Converter# [ . 	 (HandyControl.Controls.SimpleItemsControl   Items. 5b      ( 	 "HandyControl.Controls.SideMenuItem     MainMenuSearch6	 	 0HandyControl.Controls.HeaderedSimpleItemsControl  	  Header#  Z 6"   
! � Tag$! MainMenuSearchTag��5e   "   " � ToolTip#" Z 5f   "   # � Cursor$
# Hand��5g   "   $��0,30,0,0q�5h   "   $   Command$ 5c   "   !
 
 Stylet.Xaml.ActionExtension
  *SwitchScreenCommand+%   CommandParameter% 5d   "   �� *Tag+& �� RelativeSource& �� *Self+'   Icon' 5i        5j      (   Kind$( 	SearchWeb=�65   #��[  6F   5l        5n      )   MainMenuMonitoring#  Z) 6"   $! MainMenuMonitoringTag��5q   "   #" Z) 5r   "   $
# Hand��5s   "   $ 5o   "   
  *SwitchScreenCommand+% 5p   "   �� *Tag+& �� *Self+' 5t        5u      $( 
MonitorEye=�65   #��[  6G   5w        5y      *   MainMenuMap#  Z* 6"   $! MainMenuMapTag��5|   "   #" Z* 5}   "   $
# Hand��5~   "   $ 5z   "   
  *SwitchScreenCommand+% 5{   "   �� *Tag+& �� *Self+' 5        5�      $	( Lan=�65   #��[  6@   5�        5�      +   MainMenuGMap#  Z+ 6"   $! GMapTag��5�   "   #" Z+ 5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( 	MapMarker=�65   #��[  6F   5�      ~| ----------------------------------------- НОВАЯ КНОПОЧКА ----------------------------------------------------   5�      ,   MainMenuGraphics#  Z, 6"   $! Graphics��5�   "   #" Z, 5�   "   $��	0,-50,0,0q�5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( ChartBar=�65   #��[  6E   5�      �~ -------------------------------------- ДАЛЬШЕ НЕ ТРОГАТЬ -----------------------------------------------------5�        5�      -   5        $ SettingsSideMenu��5�      $��0��5�      $��2��5�   4   y�5�      �� $ OpenSettingsButton��$ 	IsChecked�  Boolean2VisibilityConverter# [ . 5�        -   CommonSettings#  Z- 6"   $! SettingsMenuCommonTag��5�   "   #" Z- 5�   "   $��0,30,0,0q�5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( ApplicationCogOutline=�65   #��[  6R   5�        5�      .   AlertSettings#  Z. 6"   $! SettingsMenuAlertTag��5�   "   #" Z. 5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�      �� 5�       	 BellCogOutline#��� 	 6)   #��[ 5�   )   5�        5�      /   SearchSettings#  Z/ 6"   $! SettingsMenuSearchTag��5�   "   #" Z/ 5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( 	SearchWeb=�65   #��[  6F   5�        5�      0   MonitoringSettings#  Z0 6"   $! SettingsMenuMonitoringTag��5�   "   #" Z0 5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( 
MonitorEye=�65   #��[  6G   5�        5�      1   About#  Z1 6"   $! SettingsMenuAboutTag��5�   "   #" Z1 5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $( Information=�65   #��[  6H   5�        5�      2   MapSettings#  Z2 6"   $! SettingsMenuMapTag��5�   "   #" Z2 5�   "   $y�Hidden=�5�   "   $
# Hand��5�   "   $ 5�   "   
  *SwitchScreenCommand+% 5�   "   �� *Tag+& �� *Self+' 5�        5�      $	( Lan=�65   #��[  6@   5�      5�      �� 5�      $��2��6   $��Bottom=�5�      $
��Left=�5�      ��5�      �� 5�      "	 ��6   .�5�      �� 
 �@��Orientation$ 
Horizontal=�68    5�      �� 5�       6*   �� $ MenuSizeButton��$ 	IsChecked� True6i   .a�5�   "   �� 
 �@��Orientation$ Vertical=�6@   5�      5�      .+�5�      d� -   5        3 d�Name$3 OpenSettingsButton��5�      $��0q�5�      4 d� 	IsChecked$4 FalseY�5�      $��12 6 6 6q�5�       
 ToggleButtonCustom#��[
 5�      / 	 )HandyControl.Controls.StatusSwitchElement5   HideUncheckedElement$
5 True��5�      6   MainMenuSettings#" Z6 5      $
# Hand��5     , 	 &HandyControl.Interactivity.Interaction7   Triggers7 5     - 	 'HandyControl.Interactivity.EventTrigger  5     8   	EventName$8 Checked��6*   , 	 &HandyControl.Interactivity.TriggerBase9   Actions.9 5     / 	 )HandyControl.Interactivity.EventToCommand  :   Command: 60   
  *OpenSettingsScreenCommand+5       5     $8 	Unchecked��6*   .9 5	       : 60   
  *CloseSettingsScreenCommand+5
     ;   CheckedElement; 5       5     $( CogOffOutline=�65   #��[  6J   .��5       $( 
CogOutline=�61   #��[  6C   5     5     5     � 5  
   $��0��6   $��1��6    5     �� 5     $��* �6    �� 5     $
��Auto �6    .+�5     �� $��0��6   $��5,5,5,5q�5      
 Stylet.Xaml.View<   Model< 5     �� *
ActiveItem+�� 5      $��1��6   #��[ 5"     ��5!     �� *
ActiveItem+5"     5#     �� 5%  
   $��1��6   $��0��6     BorderBrush#���  5&     ._�5'     d� -   5        $3 MenuSizeButton��5+     $��0q�5(     $
4 TrueY�5)     $
5 True��5*     $
��Left=�5,     $	��2 1q�5-     $
# Hand��5.     ��5/     �� 50     "	 d�6   = �� BasedOn#= [
 6?   .�51     �� 
 �@�ToolTip>   CollapseMenu# Z> 64    52     P� 53     ? P� Property? �@d�	IsChecked@ P� Value$
@ TrueY�6;   . �54  "   �� 
 �@�ToolTipA   
ExpandMenu# ZA 6<   55     57     ; 59       5:     $( UnfoldMoreVertical=�61   #��[  6K   $��16��5;  -   $��16��68   $
# Hand��6D   .��5>       $( UnfoldLessVertical=�6-   #��[  6G   $��16��5?  )   $��16��64   5@     5A     � 5F  
   $��1��6   $��1��6   #���  5G     .+�5H     �� $��1��6   $��1��6"   #���  5I     ._�5J     d� -	   5        $3 FullScreenButton��5N     $��0q�5K     $
5 True��5M     $��Right=�5O     $	��2 1q�5P     $
# Hand��5Q     4 5L     �� *IsFullScreen+��5R     �� 5S     "	 d�6    #= [
 6C   .�5T     �� 
 �@�ToolTipB   
Fullscreen# ZB 68    5U     P� 5V  "   ? �@d�	IsChecked$
@ TrueY�6?   . �5W  &   �� 
 �@�ToolTipC   WindowedMode# ZC 6@   5X  #   5Z     ; 5\       5]     $( FullscreenExit=�65   #��[  6K   $��13��5^  -   $��13��68   $
# Hand��6D   .��5a       $( 
Fullscreen=�61   #��[  6C   $��13��5b  )   $��13��64   5c     5d     5e     5f     5h     
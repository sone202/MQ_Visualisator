   M S B A M L   `   `   `  ����C  ?TPR_lab6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:TPR_lab6TPR_lab6  5          TPR_lab6.App   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      (localclr-namespace:TPR_lab6     5        �� 	Resources  5         ComboBoxNormalBorderBrush �� Color ! ComboBoxNormalBackgroundBrush # ComboBoxDisabledForegroundBrush # ComboBoxDisabledBackgroundBrush  ComboBoxDisabledBorderBrush �� 
TargetType   ComboBoxToggleButtonTemplate � ColumnDefinitions �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName	 �� Property
 �� Value �� 
TargetType �� 	ClickMode d� 	IsChecked �� Path �� RelativeSource �� Mode J� IsHitTestVisible �� SelectionBoxItem �� SelectionBoxItemTemplate �� 
IsReadOnly �� 
IsReadOnly � AllowsTransparency �� IsDropDownOpen J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation��%  &	        &	 #     &	 F     &	 i     &	 �     &	 �     7   7  7  7  7  7  7  '�� s    7  7   7  �� 5   
   $ Black��5	      �� 5
   
   $ Black��5      �� 5   
   $ White��5      �� 5   
   $ Black��5      �� 5   
   $ Black��5      �� 5   
   " d�6   .R�5      �  5      �� 5      �� 5      $��20 �6'   .+�5      �� $��Border��5      $��2��5      8��  $ 0��5      $��
1, 1, 1, 1q�5      8�� 5      �� 5       $��ButtonBorder��5#      $��1��5       $��
1, 1, 1, 1q�5!      ���#444$ 
0, 0, 0, 0��5$      $��
0, 0, 0, 0q�5%      8�� 5&      F� 5(      $��Arrow��6   $��1��5)      =����        �    �� � 	= ��[ � z �� � z     � 	=  	= 	$��Center=�5+      ���#444$��Center=�5-      5.       5/      P� 50       �@J�IsMouseOver . . �52      �� $ ButtonBorder��53      	 � ��

 �   �54      55      P� 56       �@d�	IsChecked$
 TrueY�57      . �58      �� $ ButtonBorder��59      	 � ��

 �   �5:      �� 5;      $ Arrow��5<      	 � ��

 �   �5=      5>      P� 5?       � {� .  . �5A      �� $ Border��5B      8
  	 � ��5C      �� 5D      $ ButtonBorder��5E      8
  	 � ��5F      �� 5G      $ ButtonBorder��5H      8
  	 � ��5I      �� 5J      8
  	 � ��5K      �� 5L      $ Arrow��5M      	 � ��

 �   �5N      5O      5Q      �� 5S   
   " ��6   .�5T      �� 	 �@J�SnapsToDevicePixels
 . 5U      �� 5V      	 �@�OverridesDefaultStyle
 . 5W      �� 5X      	 � ��$

 Auto=�5Y      �� 5Z      	 � ��$

 Auto=�5[      �� 5\      	 � ��
 . 5]      �� 5^      	 � ��

 �����5_      �� 5`      	 �@�FocusVisualStyle
 5a      Z� �� 5b      	 � ��
 5c      �� 5d      " ��6&   .R�5e      � .+�5f      d� $��ToggleButton��6+   $��2��5g   +   $ Press=�5h   +   |�.  8��   5j   +   �� $ IsDropDownOpen� �� *TemplatedParent+$ TwoWay=�5k   +   �� 5m      $��ContentSite��6/   $��5, 3, 23, 3q�5n   /    .  $
��Left=�5p   /   $��Center=�5q   /   #��z 5r   /   #��z 5s   /   #��z��5t   /   �� 5u      $��PART_EditableTextBox��6&   $��3, 3, 23, 3q�5v   &   # z 5w   &   $y�Hidden=�5x   &   
������ $
��Left=�5z   &   $��Center=�5{   &   |�. .��5|   7   ��5}   "   �� 5~   &   " ��66   .R�5   *   �� $��PART_ContentHost��61   |�.  5�   1   5�   '   .��5�   4   5�      � 5�      $��Popup��6$   $��Bottom=�5�   $   |�.   . #��z 5�   $   $��Slide=�5�   $   .��5�   "   � $��DropDown��6'    . #��z 5�   '   #��z 5�   '   .+�5�   &   �� $��DropDownBorder��6-   
���   �$��
0, 1, 0, 0q�5�   -   $ 0��5�   -   $��1,1,1,1q�5�   -   8�� 5�   -   �� 5�   &   $��4q�63    . .��5�   *   �� $ 	Contained=�69   5�   '   5�   #   5�      5�       5�      P� 5�       �@��HasItems .  . �5�   "   �� $ DropDownBorder��5�   )   	 � ��$
 95��5�   )   5�      P� 5�       � {� .  . �5�   "   �� 8
  	 � ��5�   )   5�      P� 5�       �@��
IsGrouping . . �5�   "   �� 	 � ��
 .  5�   )   5�      P� 5�       �@��
IsEditable . . �5�   "   �� 	 �@ 	IsTabStop
 .  5�   )   �� 5�   "   $ PART_EditableTextBox��5�   )   	 � y�$
 Visible=�5�   )   �� 5�   "   $ ContentSite��5�   )   	 � y�$
 Hidden=�5�   )   5�      5�      5�      5�      5�      5�      
   M S B A M L   `   `   `  ����  Waze    Waze.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   ���	#FFD3D3D3.��5      �  � RowDefinitions 5      �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��10q�6(   $��0��64   $
��Left=�6A   .+�5      �� $��X:��6   $��Center=�6   $��0,0,2,0q�64   �� 5      -   5         ��Name$ InputX��5      $��40��6   $��0,0,10,0q�6(   �� 5      $��Y:��6   $��Center=�6   $��0,0,2,0q�64   �� 5      -   5        $ InputY��5      $��40��6   $��0,0,10,0q�6(   �� 5      $��Ciudad:��6   $��Center=�6   $��0,0,2,0q�69   �� 5      -   5        $ InputCiudad��5      $	��100��6"   $��0,0,10,0q�6.   �� 5      -   5         ��Name$ 
BtnColocar��5      $��Colocar punto��6    68   5      � 5      -   5         �Name$ 	OuterGrid��5   I   $��1��6	   $
��Left=�6   $	��Top=�61    5      �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5      �� 5      $
��Auto �6   �� 5      $��* �6   .+�5#      �� -   5         ��Name$ CornerLabel��5#      $��0��6    $��0��6-   
������ 6=   �� 5&      -   5         ��Name$ HorizontalLabelsPanel��5&      $ 
Horizontal=�5'      $��0��5(      $��1��5)      $	��Top=�5*      $
��Left=�5+      �� 5.      -   5        $ VerticalLabelsPanel��5.      $ Vertical=�5/      $��1��50      $��0��51      $	��Top=�52      $
��Left=�53      �� 56      -	   5        	 ��Name$	 
GridCanvas��56      $��1��57      $��1��58      ���	#FFB9F6CA59      5:      5;      5<      
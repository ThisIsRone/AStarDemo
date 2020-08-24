echo "update git"
echo "................update git................"
git pull origin master

echo "..........clean up AStarDemo.............."
cd /var/www/html/AStarDemo/
rm -fr *                   
cd /var/www/html/Excavator/  
rm -fr *                                                                         

echo "..........copy AStarDemo to html.........."
cp -r /root/AStarDemo/Export/WebGl/AStarDemo/* /var/www/html/AStarDemo/
cp -r /root/AStarDemo/Export/WebGl/Excavator/* /var/www/html/Excavator/
echo "..............restart httpd..............."
systemctl restart httpd  

cd /root/AStarDemo/
echo ".................success.................."

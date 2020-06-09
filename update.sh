echo "update git"
echo "................update git................"
git pull origin master

echo "..........clean up AStarDemo.............."
cd /var/www/html/AStarDemo/
rm -fr *                                                                                             

echo "..........copy AStarDemo to html.........."
cp -r /root/AStarDemo/Export/WebGl/AStarDemo/* /var/www/html/AStarDemo/
echo "..............restart httpd..............."
systemctl restart httpd  

cd /root/AStarDemo/
echo ".................success.................."

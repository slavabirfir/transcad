@echo off
REM c:\fwtools\bin\ogr2ogr -f "MapInfo File" %1.tab %1.shp
REM "C:\Program Files\FWTools2.4.7\bin\ogr2ogr" -f "MapInfo File" "C:\Users\Slava\Desktop\ConvertShapeToTab\DATA\12122013 010843\TAB\isstr.tab" "C:\Users\Slava\Desktop\ConvertShapeToTab\DATA\12122013 010843\SHP\isstr.shp"
"C:\Program Files\FWTools2.4.7\bin\ogr2ogr.exe" -f "MapInfo File" %1 %2

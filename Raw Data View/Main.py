from os import path, walk
from model_file import *

data_path = path.join("..", "Data")

#Сохраним хедеры для файлов температур, плотности
testfileT = path.join(data_path, "2D_logT", "BIFROST_en096014_gol_lgtg_281.fits")
testfileRo = path.join(data_path, "2D_logRo", "BIFROST_en096014_gol_lgr_281.fits")

Model_file(testfileT).SaveHeader()
Model_file(testfileRo).SaveHeader()

#Отрисовка параметров температуры
for top, dirs, files in walk(path.join(data_path, "2D_logT")):
    for file in files:
        file_name = path.join(top, file)
        file = Model_file(file_name)
        file.Plot2D("T, K")
        file.Plot_heat_map("inferno")

#Отрисовка параметров плотности
for top, dirs, files in walk(path.join(data_path, "2D_logRo")):
    for file in files:
        file_name = path.join(top, file)
        file = Model_file(file_name)
        file.Plot2D("ρ, кг м^(-3)")
        file.Plot_heat_map("YlGnBu")

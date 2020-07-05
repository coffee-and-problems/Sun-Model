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

#Отрисовка нормы вектора магнитного поля
def PlotNorm(dir, files, color_map):
        with fits.open(path.join(dir, files[0])) as _file:
            data1 = _file[0].data
        with fits.open(path.join(dir, files[1])) as _file:
            data2 = _file[0].data
        with fits.open(path.join(dir, files[2])) as _file:
            data3 = _file[0].data

        data = np.sqrt(data1*data1 + data2*data2 + data3*data3)

        values = data.reshape(1554, 6930)
        heatmap(values, cmap = color_map)

        plt.gca().invert_yaxis()
        plt.xlabel('x, Mm')
        plt.ylabel('z, Mm')
        plt.show()

for top, dirs, files in walk(path.join(data_path, "2D_mag_field")):
    for dir in dirs:
        for top1, dirs1, files1 in walk(path.join(data_path, "2D_mag_field", dir)):
            for file in files1:
                file_name = path.join(top, dir, file)
                file = Model_file(file_name)
                if (file.file_name.split('_')[-2] == 'bx'):
                    file.Plot2D("x mag field")
                    file.Plot_heat_map("OrRd")

            PlotNorm(path.join(top, dir), files1, "OrRd")

from astropy.io import fits
import matplotlib.pyplot as plt
import numpy as np
from os import path
from seaborn import heatmap

class Model_file:
    def __init__(self, file):
        self.file = file
        self.file_name = file.split('\\')[-1].split('.')[0]
        self.time_step = self.file_name.split('_')[-1]
        with fits.open(self.file) as _file:
            self.data = _file[0].data

    def SaveHeader(self):
        """Сохраняет хедер файла в рабочую директорию в формате .csv"""
        with fits.open(self.file) as _file:
            header = _file[0].header._cards

        _file_name = "{}_header.csv".format(self.file_name)
        with open(_file_name, "w") as _header_file:
            for card in header:
                line = ''.join([card[0], ',', str(card[1]), ',', card[2], '\n'])
                _header_file.write(line)

    def Plot2D(self, label):
        """Рисует два среза - при x = 0, и z = -3.07768 - то есть первый пиксель соответствующей координаты
        в исходном (логарифмическом) масштабе и линейном"""

        value_z = self.data[:,0,0]
        z = np.arange(-3.07768, self.data.shape[0]*0.0698318 - 3.07768, step = 0.0698318)
        value_x = self.data[0,0,:]
        x = np.arange(self.data.shape[2]*0.0138918, step = 0.0138918)

        log_label = "log({})".format(label)

        fig, ((ax1, ax2), (ax3, ax4)) = plt.subplots(2, 2)
        fig.suptitle('{}'.format(self.file_name))
        ax1.plot(x, value_x)
        ax1.set_xlabel('x, Mm')
        ax2.set_ylabel("log(ρ, кг м^(-3))")

        ax3.plot(z, value_z)
        ax3.set_xlabel('z, Mm')
        ax2.set_ylabel(log_label)

        linear_value_x = np.power(np.full(value_x.shape, 10), value_x)
        linear_value_z = np.power(np.full(value_z.shape, 10), value_z)

        ax2.plot(x, linear_value_x)
        ax2.set_xlabel('x, Mm')
        ax2.set_ylabel(label)

        ax4.plot(z, linear_value_z)
        ax4.set_xlabel('z, Mm')
        ax4.set_ylabel(label)

        plt.show()

    def Plot_heat_map(self, color_map):
        """Рисует тепловую карту модели (график от двух координат, где значения показаны цветом)"""
        plt.suptitle('{}'.format(self.file_name))

        values = self.data.reshape(1554, 6930)
        heatmap(values, cmap = color_map)

        plt.gca().invert_yaxis()
        plt.xlabel('x, Mm')
        plt.ylabel('z, Mm')
        plt.show()

    
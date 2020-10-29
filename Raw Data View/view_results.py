import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from os import path

#results_path = r"C:\Users\julie\source\repos\Sun Model\Model Calculations\bin\Debug\Results"

#for top, dirs, files in walk(results_path):
#    for file in files:
#        data = pd.read_csv(file)
#        file_name = file.split('\\')[-1].split('.')[0]

#        fig = plt.fig()
#        fig.suptitle(file_name)
#        plt.plot(data[0], data[1])
#        plt.show()

#file = r"C:\Users\julie\source\repos\Sun Model\Model Calculations\bin\Debug\Results\BIFROST249_4.47761.csv"
file = r"C:\Users\julie\source\repos\Sun Model\Model Calculations\bin\Debug\Results\BIFROST249_3.24.csv"

file_name = file.split('\\')[-1].split('_')[-1][:-4]
data = np.genfromtxt(file, delimiter=",", names=["r", "T"])
plt.title("Длина волны: "+ file_name+'мм')
plt.xlabel("r")
plt.ylabel("Tbr")
plt.plot(data['r'], data['T'])
plt.show()
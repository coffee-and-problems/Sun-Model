## Сохраняет данные на диск в нормальном формате вместо fits ##

from os import path, walk
from model_file import *

data_path = path.join("..", "Data")

for top, dirs, files in walk(data_path):
    for dir in dirs:
        if (dir.startswith("3D")):
            for _top, _dirs, _files in walk(path.join(data_path, dir)):
                for file in _files:
                    file_name = path.join(top, dir, file)
                    _model_file = Model_file(file_name)
                    _model_file.SaveData(path.join(data_path, "CSV"))

#!/usr/bin/python
# -*- coding: utf-8 -*-

import sys
import os, platform, locale, stat
import shutil, re, string
import xml.dom.minidom as xml_dom
import glob, getopt

print("=============== gen_config_proto =================")
console_encoding = sys.getfilesystemencoding()

script_dir = os.path.dirname(os.path.realpath(__file__))
os.chdir(os.path.join(script_dir, '..', '..'))
resource_repo_dir = os.getcwd();
print(script_dir)
print(resource_repo_dir)

pb_file_dir = os.path.join(resource_repo_dir, 'config')
pb_file_path = os.path.join(pb_file_dir, 'target','config.all.pb')
proto_file_dir = os.path.join(resource_repo_dir, 'protocol', 'config')
excel_file_dir = os.path.join(resource_repo_dir, 'config', 'excel')

data_file_dir = pb_file_dir
pb_file_patterns = ['*.proto']
pb_file_patterns_default = True
xresloader_file = os.path.join(resource_repo_dir, 'tools', 'xresloader', 'xresloader.jar')

# 自动创建文件夹
auto_make_dirs = [pb_file_dir, proto_file_dir, excel_file_dir, data_file_dir]
for dir_path in auto_make_dirs:
    if not os.path.exists(dir_path):
        os.makedirs(dir_path)

# 查找协议文件
proto_file_list = []

os.chdir(proto_file_dir)

for pb_file_pattern in pb_file_patterns:
    for filename in glob.glob(pb_file_pattern):
        proto_file_list.append('"' + filename + '"')

# 先生成描述文件
if 'Windows' == platform.system() or 'cygwin' == platform.system()[0:6].lower() or 'msys' == platform.system()[0:4].lower() :
    tools_os_dir = "win32"
elif 'Linux' == platform.system():
    tools_os_dir = 'linux_x86_64'
elif 'Darwin' == platform.system():
    tools_os_dir = 'macos_x86_64'
else:
    print('[ERROR] platform ' + platform.system() + ' not supported')
    exit(0)

protoc_path = ''
for exec_bin_path in glob.glob(os.path.join(resource_repo_dir, 'tools', 'protobuf', tools_os_dir, 'bin', 'protoc*')):
    os.chmod(exec_bin_path, stat.S_IRWXU + stat.S_IRWXG + stat.S_IRWXO)
    protoc_path = exec_bin_path

cmd = protoc_path + \
        ''.join([' --proto_path . -o "', pb_file_path, '" ']) + \
        ' '.join(proto_file_list)

print('[cd]  : ' + os.getcwd())
print('[exec]: ' + cmd)
ret_code = os.system(cmd)
if 'Windows' == platform.system() and ret_code != 0:
    print("[ERROR] 尝试转换协议描述数据失败")
    try:
        input('Press Enter to continue...')
    except SyntaxError:
        pass

    exit(0)

print("convert protocol desc file done.")
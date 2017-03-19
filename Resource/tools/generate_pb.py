#!/usr/bin/bash python
# -*- coding: utf-8 -*-

import sys
import os, platform, locale, stat
import shutil, re, string, glob, subprocess
from optparse import OptionParser

print(" ======================================== ")

# ============================ Set Pathes
console_encoding = sys.getfilesystemencoding()
script_dir = os.path.dirname(os.path.realpath(__file__))
os.chdir(script_dir)
resource_repo_dir = os.path.join(script_dir, '..')
client_project_dir = os.path.join(resource_repo_dir,'..','Client',)


convert_list_file = os.path.join(resource_repo_dir,'config','convert_list.xml')

#Client 相关路径
p_client_table = os.path.join(client_project_dir, 'unity_project', 'Assets', 'Docs', 'Table')
p_client_lua_data = os.path.join(client_project_dir, 'unity_project', 'Assets', 'Docs', 'luaScript', 'data', 'const')
p_client_protocol = os.path.join(client_project_dir, 'unity_project', 'Assets', 'Docs', 'Protocol')

# xresloader
xresloader_file = os.path.join(resource_repo_dir, 'tools', 'xresloader', 'xresloader.jar')
xresconv_file = os.path.join(resource_repo_dir, 'tools', 'xresconv', 'xresconv-cli.py')
xresconv_gen_allpb_file = os.path.join(resource_repo_dir, 'tools', 'xresconv', 'gen_config_proto.py')
xresconv_pb_file_dir = os.path.join(resource_repo_dir, 'config', 'target')


#proto工具路径
p_protoc = ""
if 'Windows' == platform.system() or 'cygwin' == platform.system()[0:6].lower() or 'msys' == platform.system()[0:4].lower() :
    p_protoc = os.path.join(resource_repo_dir, os.path.normcase("tools/protobuf/win32/bin/protoc.exe"))
elif 'Linux' == platform.system():
    p_protoc = os.path.join(resource_repo_dir, "tools/protobuf/linux_x86_64/bin/protoc")
elif 'Darwin' == platform.system():
    p_protoc = os.path.join(resource_repo_dir, os.path.normcase("tools/protobuf/macos_x86_64/bin/protoc"))
else:
    print('[ERROR] platform ' + platform.system() + ' not supported')
    exit(0)


# 自动创建文件夹
auto_make_dirs = [p_client_table, p_client_protocol, p_client_lua_data]
for dir_path in auto_make_dirs:
    if not os.path.exists(dir_path):
        os.makedirs(dir_path)


ret_code = 0
# ============================ Generate Excel (pb => clientDoc/protocal & bin => clientDoc/table )
cmd_gen_excel_pb = 'python "{0}"'.format(xresconv_gen_allpb_file)
print(cmd_gen_excel_pb)
if ret_code == 0:
    ret_code = os.system(cmd_gen_excel_pb)
else:
    os.system(cmd_gen_excel_pb)

print(" =================== Start Convert Excel to Proto ===================== ")
for item in glob.glob(os.path.join(xresconv_pb_file_dir, '*.pb')):
    filename = os.path.basename(item)
    dst = os.path.join(p_client_protocol, filename)
    if os.path.isfile(dst):
        os.remove(dst)
    shutil.copy(item, dst)

cmd_gen_excel_bin = 'python "{0}" "{1}" -o "{2}"'.format(xresconv_file, convert_list_file, p_client_table)
print(cmd_gen_excel_bin)
if ret_code == 0:
    ret_code = os.system(cmd_gen_excel_bin)
else:
    os.system(cmd_gen_excel_bin)


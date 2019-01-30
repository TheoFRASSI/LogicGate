#-------------------------------------------------
#
# Project created by QtCreator 2019-01-12T12:27:11
#
#-------------------------------------------------

QT       += core gui

CONFIG += c++14

LIBS += -lglu32 -lopengl32

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = TP2-PGRA
TEMPLATE = app


SOURCES += main.cpp\
        princ.cpp \
        glarea.cpp

HEADERS  += princ.h \
        glarea.h

FORMS    += princ.ui

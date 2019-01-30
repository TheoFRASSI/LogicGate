#ifndef PRINC_H
#define PRINC_H

#include "ui_princ.h"

class Princ : public QMainWindow, private Ui::Princ
{
    Q_OBJECT

public:
    explicit Princ(QWidget *parent = nullptr);

public slots:
    void setSliderRadius(double radius);

protected slots:
    void onSliderRadius(int value);
};

#endif // PRINC_H

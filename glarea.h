#ifndef GLAREA_H
#define GLAREA_H

#include <QOpenGLWidget>
#include <QOpenGLFunctions>
#include <QKeyEvent>
#include <QTimer>

class GLArea : public QOpenGLWidget,
               protected QOpenGLFunctions
{
    Q_OBJECT

public:
    explicit GLArea(QWidget *parent = nullptr);
    ~GLArea() override;
    struct cylindre{
        GLfloat ep_cyl; //AB
        GLfloat r_cyl;  //AC dans xy
        int nb_fac;
        GLfloat r, g, b;
        GLfloat x, y, z;
        cylindre(GLfloat ep_cyl, GLfloat r_cyl, int nb_fac, GLfloat r, GLfloat g, GLfloat b, GLfloat x, GLfloat y, GLfloat z)
                :ep_cyl(ep_cyl),r_cyl(r_cyl),nb_fac(nb_fac),r(r),g(g),b(b),x(x),y(y),z(z){}
    };
   cylindre cyl_piston = cylindre(1.2f, 0.2f, 60, 1, 0.5, 0.5, -2.f, 0, 0.4f);
   cylindre cyl_KJ = cylindre(1.f, 0.05f, 40, 1, 0, 1, -1.f, 0, 0.4f);
   cylindre cyl_extr_KJ = cylindre(0.2f, 0.075f, 60, 1, 0, 1, -0.4f, 0, 0.4f);
   cylindre cyl_extrG_JH = cylindre(0.2f, 0.1f, 60, 0.5, 1, 0.5, -0.4f, 0, 0.2f);
   cylindre cyl_extrD_JH = cylindre(0.2f, 0.1f, 60, 0.5, 1, 0.5, 0.4f, 0, 0.2f);
   cylindre cyl_JH = cylindre(0.8f, 0.075f, 40, 0, 1, 0, 0, 0, 0.2f);
   cylindre cyl_J = cylindre(0.42f, 0.05f, 20, 0, 1, 0, -0.4f, 0, 0.3f);
   cylindre cyl_H = cylindre(0.42f, 0.05f, 20, 0, 1, 0, 0.4f, 0, 0.1f);
   cylindre cyl_G = cylindre(0.42f, 0.05f, 20, 0, 0, 1, 0, 0, -0.1f);
   cylindre cyl_roue = cylindre(0.2f, 0.5, 60, 0, 1, 1, 0, 0, 0);

public slots:
    void setRadius(GLdouble radius);

signals:  // On ne les implémente pas, elles seront générées par MOC ;
          // les paramètres seront passés aux slots connectés.
    void radiusChanged(GLdouble newRadius);

protected slots:
    void onTimeout();

protected:
    void initializeGL() override;
    void doProjection();
    void resizeGL(int w, int h) override;
    void dessinerCylindre(cylindre cyl, bool vertical);
    void dessinerFacette(cylindre cyl, bool vertical);
    void paintGL() override;
    void keyPressEvent(QKeyEvent *ev) override;
    void keyReleaseEvent(QKeyEvent *ev) override;
    void mousePressEvent(QMouseEvent *ev) override;
    void mouseReleaseEvent(QMouseEvent *ev) override;
    void mouseMoveEvent(QMouseEvent *ev) override;

private:
    GLfloat m_angle = 0;
    QTimer *m_timer = nullptr;
    GLfloat m_alpha = 0;
    GLdouble m_radius = 0.5;
    GLdouble m_ratio = 1;
};

#endif // GLAREA_H

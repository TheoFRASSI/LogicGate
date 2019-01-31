#include "glarea.h"
#include <QDebug>
#include <QSurfaceFormat>
#include <gl/GL.h>
#include <gl/glu.h>
#include <QOpenGLFramebufferObjectFormat> // Antialiasing
#include <QtMath>

GLArea::GLArea(QWidget *parent) :
    QOpenGLWidget(parent)
{
    //qDebug() << "init GLArea" ;

    // Ce n'est pas indispensable
    QSurfaceFormat sf;
    sf.setDepthBufferSize(24);
    sf.setSamples(16); // antialiasing
    setFormat(sf);
    //qDebug() << "Depth is"<< format().depthBufferSize();

    setEnabled(true);  // événements clavier et souris
    setFocusPolicy(Qt::StrongFocus); // accepte focus
    setFocus();                      // donne le focus

    m_timer = new QTimer(this);
    m_timer->setInterval(50);  // msec
    connect (m_timer, SIGNAL(timeout()), this, SLOT(onTimeout()));
    connect (this, SIGNAL(radiusChanged(GLdouble)), this, SLOT(setRadius(GLdouble)));
}

GLArea::~GLArea()
{
    //qDebug() << "destroy GLArea";

    delete m_timer;

    // Contrairement aux méthodes virtuelles initializeGL, resizeGL et repaintGL,
    // dans le destructeur le contexte GL n'est pas automatiquement rendu courant.
    makeCurrent();

    // ici destructions de ressources GL

    doneCurrent();
}


void GLArea::initializeGL()
{
    //qDebug() << __FUNCTION__ ;
    initializeOpenGLFunctions();
    glEnable(GL_DEPTH_TEST);
}

void GLArea::doProjection()
{
    glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
    GLdouble hr = m_radius, wr = hr * m_ratio;
    glFrustum(-wr, wr, -hr, hr, 1.0, 5.0);
    glMatrixMode(GL_MODELVIEW);
}

void GLArea::resizeGL(int w, int h)
{
    //qDebug() << __FUNCTION__ << w << h;

    // C'est fait par défaut
    glViewport(0, 0, w, h);

    m_ratio = static_cast<GLdouble>(w) / h;
    doProjection();
}

void GLArea::dessinerCylindre(cylindre cyl, bool vertical){
    GLdouble a = 2 * 3.141592 / cyl.nb_fac;
    for(int i = 0; i < cyl.nb_fac; i++){
        glPushMatrix();
        if(vertical){
            glRotatef(90, 0, 1, 0);
        }
        glRotatef(360/cyl.nb_fac * i, 0.0, 0.0, 1.0);
        glBegin(GL_POLYGON);
            glColor3f (cyl.r, cyl.g, cyl.b);
            glVertex3f(0, 0, static_cast<GLfloat>(cyl.ep_cyl/2));
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(0)), cyl.r_cyl * static_cast<GLfloat>(sin(0)), cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(a)), cyl.r_cyl * static_cast<GLfloat>(sin(a)), cyl.ep_cyl/2);
        glEnd();
        glBegin(GL_POLYGON);
            glVertex3f(0, 0, -cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(0)), cyl.r_cyl * static_cast<GLfloat>(sin(0)), -cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(a)), cyl.r_cyl * static_cast<GLfloat>(sin(a)), -cyl.ep_cyl/2);
        glEnd();
        glPopMatrix();
    }
    dessinerFacette(cyl, vertical);
}

void GLArea::dessinerFacette(cylindre cyl, bool vertical){
    for(int i = 0; i < cyl.nb_fac; i++){
        glPushMatrix();
        if(vertical){
            glRotatef(90, 0, 1, 0);
        }
        glRotatef(360/cyl.nb_fac * i, 0.0, 0.0, 1.0);
        GLfloat a = 2 * 3.141592f / cyl.nb_fac;
        glColor3f (cyl.r*0.8f, cyl.g*0.8f, cyl.b*0.8f);
        glBegin(GL_QUADS);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(a)), cyl.r_cyl * static_cast<GLfloat>(sin(a)), -cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(a)), cyl.r_cyl * static_cast<GLfloat>(sin(a)), cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(0)), cyl.r_cyl * static_cast<GLfloat>(sin(0)), cyl.ep_cyl/2);
            glVertex3f(cyl.r_cyl * static_cast<GLfloat>(cos(0)), cyl.r_cyl * static_cast<GLfloat>(sin(0)), -cyl.ep_cyl/2);
        glEnd();
        glPopMatrix();
    }

}

void GLArea::paintGL()
{
    //qDebug() << __FUNCTION__ ;
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    glLoadIdentity();
    gluLookAt (0, 0, 3.0, 0, 0, 0, 0, 1, 0);
    glRotatef(m_alpha,0.1,1.0,0.0);

    float GH, HJ2, HI, JI;
    GH = sqrt(pow(cyl_H.posx - cyl_G.posx, 2) + pow(cyl_H.posy - cyl_G.posy, 2));
    HJ2 = pow(cyl_J.posx - cyl_H.posx,2) + pow(cyl_J.posy - cyl_H.posy,2); // HJ2 = IJ2 + HI2
    HI = sqrt(pow(cyl_H.x - cyl_H.x,2) + pow(0.f - cyl_H.y, 2));
    JI = sqrt(pow(cyl_H.x - cyl_J.x, 2) + pow(0.f - cyl_J.y, 2));

    glPushMatrix();
        glRotatef(-m_angle,0,0,1);
        glTranslatef(cyl_roue.x, cyl_roue.y, cyl_roue.z);
        dessinerCylindre(cyl_roue, false);
    glPopMatrix();

    glPushMatrix();
        glTranslatef(cyl_G.x, cyl_G.x, cyl_G.z);
        dessinerCylindre(cyl_G, false);
    glPopMatrix();

    glPushMatrix();
        cyl_H.x = (cyl_roue.x + cyl_roue.r_cyl - cyl_extrD_JH.r_cyl) * cos(qDegreesToRadians(-m_angle));
        cyl_H.y = (cyl_roue.y + cyl_roue.r_cyl - cyl_extrD_JH.r_cyl) * sin(qDegreesToRadians(-m_angle));
        glTranslatef(cyl_H.x, cyl_H.y, cyl_H.z);
        dessinerCylindre(cyl_H, false);
    glPopMatrix();

    glPushMatrix();
        cyl_J.x = cyl_H.x - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2));
        glTranslatef(cyl_J.x, cyl_J.y, cyl_J.z);
        dessinerCylindre(cyl_J, false);
    glPopMatrix();

    glPushMatrix();
        cyl_JH.x =(cyl_J.x + cyl_H.x)/2.f;
        cyl_JH.y =(cyl_J.y + cyl_H.y)/2.f;

        glTranslatef(cyl_JH.x, cyl_JH.y, cyl_JH.z);

        if(cyl_H.y < 0.f){
            glRotatef(qRadiansToDegrees(-atan(HI/JI)),0.0,0.0,1.0);
        } else if(cyl_H.y == 0.f){
            glRotatef(0.f,0.0,0.0,1.0);
        } else {
            glRotatef(qRadiansToDegrees(atan(HI/JI)),0.0,0.0,1.0);
        }
        dessinerCylindre(cyl_JH, true);
    glPopMatrix();

    glPushMatrix();
        cyl_KJ.x =cyl_KJ.posx / 2 + cyl_H.x - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2));
        glTranslatef(cyl_KJ.x, cyl_KJ.y, cyl_KJ.z);
        dessinerCylindre(cyl_KJ, true);
    glPopMatrix();

    glPushMatrix();
        glTranslatef(cyl_piston.x, cyl_piston.y, cyl_piston.z);
        dessinerCylindre(cyl_piston, true);
    glPopMatrix();

    glPushMatrix();
        cyl_extr_KJ.x = cyl_H.x - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2));
        glTranslatef(cyl_extr_KJ.x, cyl_extr_KJ.y, cyl_extr_KJ.z);
        dessinerCylindre(cyl_extr_KJ, false);
    glPopMatrix();

    glPushMatrix();
        cyl_extrD_JH.x = (cyl_roue.x + cyl_roue.r_cyl - cyl_extrD_JH.r_cyl) * cos(qDegreesToRadians(-m_angle));
        cyl_extrD_JH.y = (cyl_roue.y + cyl_roue.r_cyl - cyl_extrD_JH.r_cyl) * sin(qDegreesToRadians(-m_angle));
        glTranslatef(cyl_extrD_JH.x, cyl_extrD_JH.y, cyl_extrD_JH.z);
        dessinerCylindre(cyl_extrD_JH, false);
    glPopMatrix();

    glPushMatrix();
        cyl_extrG_JH.x = cyl_H.x - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2));
        glTranslatef(cyl_extrG_JH.x, cyl_extrG_JH.y, cyl_extrG_JH.z);
        dessinerCylindre(cyl_extrG_JH, false);
    glPopMatrix();
}

//void GLArea::on_timer (int id)
//{
//    if (!anim_flag) return;
//    anim_angle += 1;
//    if (anim_angle >= 360) m_angle = 0;
//    paintGL();
//    QTimer* timer = new QTimer();
//    timer->singleShot(anim_delay,this,on_timer); // on relance le même timer
//}

//void GLArea::on_timer_rotate(int id)
//{
//    if (!anim_rotate) return;
//    m_angle ++;
//    paintGL();
//    glutTimerFunc(anim_delay_rotate, on_timer_rotate, id);  // on relance le même timer
//}

void GLArea::keyPressEvent(QKeyEvent *ev)
{
    //qDebug() << __FUNCTION__ << ev->text();

    switch(ev->key()) {
        case Qt::Key_Space :
            m_angle += 5;
            if (m_angle >= 360) m_angle = 0;
            update();
            break;
        case Qt::Key_A :
                    anim_flag = !anim_flag;
                    if (anim_flag) glutTimerFunc (anim_delay, on_timer, 0);
                    break;
        case Qt::Key_R :
            if (ev->text() == "r")
                 setRadius(m_radius-0.05);
            else setRadius(m_radius+0.05);
            break;
    }
}

void GLArea::keyReleaseEvent(QKeyEvent *ev)
{
    //qDebug() << __FUNCTION__ << ev->text();
}

void GLArea::mousePressEvent(QMouseEvent *ev)
{
    //qDebug() << __FUNCTION__ << ev->x() << ev->y() << ev->button();
}

void GLArea::mouseReleaseEvent(QMouseEvent *ev)
{
    //qDebug() << __FUNCTION__ << ev->x() << ev->y() << ev->button();
}

void GLArea::mouseMoveEvent(QMouseEvent *ev)
{
//    qDebug() << __FUNCTION__ << ev->x() << ev->y();
}

void GLArea::onTimeout()
{
//    qDebug() << __FUNCTION__ ;
    m_alpha += 0.01f;
    if (m_alpha > 1) m_alpha = 0;
    update();
}

void GLArea::setRadius(GLdouble radius)
{
    //qDebug() << __FUNCTION__ << radius << sender();
    if ( !(fabs(radius - m_radius) < DBL_EPSILON) && radius > 0.01 && radius <= 10) {
        m_radius = radius;
        //qDebug() << "  emit radiusChanged()";
        emit radiusChanged(radius);
        makeCurrent();
        doProjection();
        doneCurrent();
        update();
    }
}





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
    qDebug() << "init GLArea" ;

    // Ce n'est pas indispensable
    QSurfaceFormat sf;
    sf.setDepthBufferSize(24);
    sf.setSamples(16); // antialiasing
    setFormat(sf);
    qDebug() << "Depth is"<< format().depthBufferSize();

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
    qDebug() << "destroy GLArea";

    delete m_timer;

    // Contrairement aux méthodes virtuelles initializeGL, resizeGL et repaintGL,
    // dans le destructeur le contexte GL n'est pas automatiquement rendu courant.
    makeCurrent();

    // ici destructions de ressources GL

    doneCurrent();
}


void GLArea::initializeGL()
{
    qDebug() << __FUNCTION__ ;
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
    qDebug() << __FUNCTION__ << w << h;

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
    qDebug() << __FUNCTION__ ;
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

    glLoadIdentity();
    gluLookAt (0, 0, 3.0, 0, 0, 0, 0, 1, 0);
    //glRotatef(m_angle,0.1,1.0,0.0);

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
        glRotatef(-m_angle,0,0,1.0);
        glTranslatef(cyl_H.x, cyl_H.y, cyl_H.z);
        dessinerCylindre(cyl_H, false);
    glPopMatrix();

    glPushMatrix();

        float GH, HJ2;
        GH = sqrt(pow(cyl_H.x-cyl_G.x,2) + pow(cyl_H.y-cyl_G.y,2));
        HJ2 = pow(cyl_J.x-cyl_H.x,2) + pow(cyl_J.y-cyl_H.y,2); // HJ2 = IJ2 + HI2
        /*float xi = cyl_H.x;
        float yi = cyl_G.y; // = 0
        float xj = xi - sqrt(HJ2 - pow(GH*sin(m_angle),2));
        float yj = yi; // = 0*/

        //glTranslatef(cyl_H.x - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(-m_angle)),2)), cyl_G.y, cyl_J.z);
        glTranslatef(cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2)), cyl_G.y, cyl_J.z);
        //glTranslatef(cyl_J.x, cyl_J.y, cyl_J.z);
        dessinerCylindre(cyl_J, false);
    glPopMatrix();

    glPushMatrix();
        glRotatef(atan((cyl_H.y * sin(qDegreesToRadians(m_angle)))/abs((cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2))) - (cyl_H.x * cos(qDegreesToRadians(m_angle))))),0,0,1.0);
        glTranslatef((cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2)) + (cyl_H.x * cos(qDegreesToRadians(m_angle))))/2, (cyl_J.y + cyl_H.y * sin(qDegreesToRadians(m_angle)))/2, cyl_JH.z);
        dessinerCylindre(cyl_JH, true);
    glPopMatrix();

    glPushMatrix();
        glTranslatef(cyl_KJ.x / 2 + cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2)), cyl_G.y, cyl_KJ.z);
        dessinerCylindre(cyl_KJ, true);
    glPopMatrix();

    glPushMatrix();
        glTranslatef(cyl_piston.x, cyl_piston.y, cyl_piston.z);
        dessinerCylindre(cyl_piston, true);
    glPopMatrix();

    glPushMatrix();
        //glTranslatef(cyl_extr_KJ.x, cyl_extr_KJ.y, cyl_extr_KJ.z);
    glTranslatef(cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2)), cyl_G.y, cyl_extr_KJ.z);
        dessinerCylindre(cyl_extr_KJ, false);
    glPopMatrix();

    glPushMatrix();
        glRotatef(-m_angle,0,0,1.0);
        glTranslatef(cyl_extrD_JH.x, cyl_extrD_JH.y, cyl_extrD_JH.z);
        dessinerCylindre(cyl_extrD_JH, false);
    glPopMatrix();

    glPushMatrix();
        glTranslatef(cyl_H.x * cos(qDegreesToRadians(m_angle)) - sqrt(HJ2 - pow(GH * sin(qDegreesToRadians(m_angle)),2)), cyl_G.y, cyl_extrG_JH.z);
        //glTranslatef(cyl_extrG_JH.x, cyl_extrG_JH.y, cyl_extrG_JH.z);
        dessinerCylindre(cyl_extrG_JH, false);
    glPopMatrix();
}

void GLArea::keyPressEvent(QKeyEvent *ev)
{
    qDebug() << __FUNCTION__ << ev->text();

    switch(ev->key()) {
        case Qt::Key_Space :
            m_angle += 2;
            if (m_angle >= 360) m_angle = 0;
            update();
            break;
        case Qt::Key_A :
            if (m_timer->isActive())
                m_timer->stop();
            else m_timer->start();
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
    qDebug() << __FUNCTION__ << radius << sender();
    if ( !(fabs(radius - m_radius) < DBL_EPSILON) && radius > 0.01 && radius <= 10) {
        m_radius = radius;
        qDebug() << "  emit radiusChanged()";
        emit radiusChanged(radius);
        makeCurrent();
        doProjection();
        doneCurrent();
        update();
    }
}





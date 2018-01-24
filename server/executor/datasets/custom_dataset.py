import os
import random
import cv2
import numpy as np

IMG_EXTS = ['jpg', 'png', 'jpeg', 'bmp']

PATH = os.path.dirname(__file__)

def get_custom_dataset(path, shape = (224, 224), train_val_ratio = 0.7):
    # return: a tensor whose shape is (N,C,H,W), H, W = shape
    if len(os.path.dirname(path)) == 0:
        path = os.path.join(PATH, path)
    dirs = os.listdir(path)
    cls_imgs = dict()
    for cls_name in dirs:
        p = os.path.join(path, cls_name)
        for name in os.listdir(p):
            ext = name.split('.')[-1]
            if ext not in IMG_EXTS:
                continue
            # the file is a picture
            if cls_name not in cls_imgs:
                cls_imgs[cls_name] = []
            cls_imgs[cls_name].append(name)
    num_cls = len(cls_imgs.keys())
    num_imgs = len(cls_imgs)
    raw = []
    for gt, cls_name in enumerate(cls_imgs.keys()):
        for img_name in cls_imgs[cls_name]:
            fname = os.path.join(path, cls_name, img_name)
            im = cv2.imread(fname) # H, W, C
            if len(im.shape) == 2:
                im = np.stack([im,im,im])
            # center crop
            h, w = im.shape[:2]
            th, tw = shape
            yw = th * w * 1.0 / h
            
            if yw > tw:
                ch = th
                cw = int(yw)
            else:
                cw = tw
                ch = int(tw * h * 1.0 / w)

            im = cv2.resize(im, (cw, ch))
            dw = cw - tw
            dh = ch - th
            ow = dw // 2
            oh = dh // 2
            im = im[oh:oh + th, ow:ow + tw, :] # .transpose((2,0,1))
            raw.append((im, gt))
            
    random.shuffle(raw)
    t = int(train_val_ratio * len(raw))

    data = np.array([d[0] for d in raw]) / 255.0
    label = np.array([d[1] for d in raw])
    
    x_train = data[:t]
    x_test = data[t:]
    
    def onehot(x, num_cls):
        return np.eye(num_cls)[x.reshape(-1)]
        
    y_train = onehot(label[:t], num_cls)
    y_test = onehot(label[t:], num_cls)
    return x_train, y_train, x_test, y_test
    
if __name__ == '__main__':
    path = './little_dog_cat/'
    get_custom_dataset(path)
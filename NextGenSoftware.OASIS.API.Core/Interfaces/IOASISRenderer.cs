﻿

namespace NextGenSoftware.OASIS.API.Core
{
    public interface IOASISRenderer : IOASISProvider
    {
        void DrawSprite(object sprite, float x, float y);
        void Draw3DObject(object obj, float x, float y);
    }
}

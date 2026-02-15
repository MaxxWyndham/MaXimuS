using System.ComponentModel;

namespace MaXimuS.Models
{
    [DisplayName("SCENE")]
    public class Scene
    {
        [DisplayName("SCENE_FILENAME")]
        public string? FileName { get; set; }

        [DisplayName("SCENE_FIRSTFRAME")]
        public int FirstFrame { get; set; } = 0;

        [DisplayName("SCENE_LASTFRAME")]
        public int LastFrame { get; set; } = 100;

        [DisplayName("SCENE_FRAMESPEED")]
        public int FrameSpeed { get; set; } = 30;

        [DisplayName("SCENE_TICKSPERFRAME")]
        public int TicksPerFrame { get; set; } = 160;

        [DisplayName("SCENE_BACKGROUND_STATIC")]
        public Vector3 BackgroundStatic { get; set; } = Vector3.Zero;

        [DisplayName("SCENE_AMBIENT_STATIC")]
        public Vector3 AmbientStatic { get; set; } = new(0.04314f);
    }
}

-- DRACORIA EVOLVED | BUILD 015
-- Codea Modern. No external assets required.
-- Temporary procedural preview. Real sprite atlases are the next milestone.
local mode="creator"
local hero={x=0,y=0,dir=0,walk=0}
local appearance={race=1,body=1,face=1,hair=1,hairColor=1,skin=2,eyes=1,outfit=1}
local cats={
 {"RACE","race",{"Human","Elf","Dwarf","Draconian"}},
 {"BODY","body",{"Balanced","Athletic","Heavy","Slim"}},
 {"FACE","face",{"Heroic","Youthful","Rugged","Sharp"}},
 {"HAIR","hair",{"Wolf Cut","Short","Long","Spiky"}},
 {"HAIR COLOR","hairColor",{"Silver","Black","Brown","Blonde"}},
 {"SKIN","skin",{"Fair","Tan","Brown","Deep"}},
 {"EYES","eyes",{"Blue","Green","Amber","Violet"}},
 {"OUTFIT","outfit",{"Knight","Rogue","Mage","Adventurer"}}
}
local selected=1
local facing=0
local joy=nil
local skins={{244,207,173},{215,163,120},{157,101,72},{92,58,48}}
local hairs={{224,231,239},{38,36,47},{120,76,50},{222,183,91}}
local outfits={{39,77,150},{67,76,79},{100,60,145},{109,85,62}}
local eyes={{65,157,230},{77,181,112},{220,159,55},{165,105,220}}
local function color3(c,a) style.fill(c[1],c[2],c[3],a or 255) end
local function box(x,y,w,h,c) color3(c) rect(x,y,w,h) end
local function hit(t,x,y,w,h) return t.x>=x and t.x<=x+w and t.y>=y and t.y<=y+h end
local function button(s,x,y,w,h,active)
 box(x,y,w,h,active and {59,99,155} or {29,42,63})
 style.fill(255) text(s,x+w/2,y+h/2)
end
local function iso(x,y) return (x-y)*40,(x+y)*20 end
-- Single renderer used by BOTH creator and game.
local function drawHero(cx,feet,scale,dir,walking)
 local skin=skins[appearance.skin]
 local hair=hairs[appearance.hairColor]
 local armor=outfits[appearance.outfit]
 local eye=eyes[appearance.eyes]
 local back=(dir==3 or dir==4 or dir==5)
 local side=(dir==2 or dir==6)
 local dwarf=(appearance.race==3)
 local shift=dwarf and -4 or 0
 local stride=walking and math.floor(math.sin(hero.walk*11)*2) or 0
 local bw=appearance.body==3 and 17 or (appearance.body==4 and 10 or 13)
 local function p(x,y,w,h,c) box(cx+(x-16)*scale,feet+(y+shift)*scale,w*scale,h*scale,c) end
 style.fill(0,0,0,75) ellipse(cx,feet,21*scale,6*scale)
 p(10,2+stride,5,13,{39,42,57});p(18,2-stride,5,13,{39,42,57})
 p(9,2+stride,7,5,{64,49,47});p(18,2-stride,7,5,{64,49,47})
 if appearance.outfit==1 or appearance.outfit==3 then p(7,12,18,20,{28,42,86}) end
 p(16-bw/2,15,bw,17,armor)
 p(14,17,4,12,appearance.outfit==1 and {226,181,79} or {135,135,145})
 p(16-bw/2-4,18+stride,4,12,armor);p(16+bw/2,18-stride,4,12,armor)
 p(16-bw/2-4,15+stride,4,4,skin);p(16+bw/2,15-stride,4,4,skin)
 p(14,31,5,4,skin);p(9,33,14,12,skin)
 if appearance.race==2 then p(6,38,3,5,skin);p(23,38,3,5,skin) end
 if appearance.race==4 then p(7,43,3,7,{190,160,85});p(22,43,3,7,{190,160,85}) end
 if not back then
  if side then p(dir==2 and 19 or 11,39,2,2,eye)
  else p(12,39,2,2,eye);p(19,39,2,2,eye) end
 end
 p(8,43,16,5,hair)
 if appearance.hair==1 then
  p(7,38,4,10,hair);p(21,38,4,10,hair);p(8,31,4,8,hair);p(20,31,4,8,hair)
 elseif appearance.hair==2 then p(8,41,3,5,hair)
 elseif appearance.hair==3 then p(7,28,4,19,hair);p(21,28,4,19,hair)
 else p(10,46,3,6,hair);p(16,46,3,8,hair);p(22,46,3,6,hair) end
 if back then p(9,36,14,9,hair) end
end
local function drawCreator()
 background(15,22,36)
 style.fill(239,207,136) text("DRACORIA EVOLVED",WIDTH/2,HEIGHT-36)
 style.fill(255) text("CREATE YOUR HERO",WIDTH/2,HEIGHT-69)
 local bottom=HEIGHT*.45
 box(16,bottom,WIDTH-32,HEIGHT*.39,{30,43,62})
 style.fill(104,109,122) ellipse(WIDTH/2,bottom+44,154,36)
 local scale=math.min(3.1,HEIGHT*.39/69)
 drawHero(WIDTH/2,bottom+45,scale,facing,false)
 button("< TURN",25,bottom+8,90,40,false)
 button("TURN >",WIDTH-115,bottom+8,90,40,false)
 local c=cats[selected]
 style.fill(239,207,136) text(c[1],WIDTH/2,bottom-25)
 local y=bottom-93;local gap=5;local w=(WIDTH-36-3*gap)/4
 for i=1,4 do button(c[3][i],18+(i-1)*(w+gap),y,w,48,appearance[c[2]]==i) end
 button("PREVIOUS",18,y-61,(WIDTH-42)/2,45,false)
 button("NEXT",WIDTH/2+3,y-61,(WIDTH-42)/2,45,false)
 style.fill(180,190,210) text(selected.." / "..#cats,WIDTH/2,y-87)
 button("ENTER WORLD",20,20,WIDTH-40,60,true)
end
local function drawGame(dt)
 background(49,100,71)
 local mx,my=0,0
 if joy then mx,my=joy.dx,joy.dy end
 local magnitude=math.sqrt(mx*mx+my*my)
 local moving=magnitude>.08
 if moving then
  hero.x=hero.x+mx*dt*3
  hero.y=hero.y+my*dt*3
  hero.walk=hero.walk+dt
  local sx,sy=iso(mx,my)
  local angle=math.atan(sy,sx)
  hero.dir=math.floor((angle+math.pi/2+math.pi/8)/(math.pi/4))%8
 end
 local camx,camy=iso(hero.x,hero.y)
 local gx,gy=math.floor(hero.x),math.floor(hero.y)
 for y=gy-13,gy+13 do for x=gx-13,gx+13 do
  local sx,sy=iso(x,y)
  sx=WIDTH/2+sx-camx;sy=HEIGHT*.53+sy-camy
  local road=math.abs(x)<=1 or math.abs(y)<=1
  local c=road and {117,107,93} or (((x+y)%2==0) and {64,122,75} or {73,136,83})
  style.stroke(c[1],c[2],c[3]);style.strokeWidth(20)
  line(sx-20,sy,sx,sy+10);line(sx,sy+10,sx+20,sy)
  line(sx+20,sy,sx,sy-10);line(sx,sy-10,sx-20,sy)
 end end
 drawHero(WIDTH/2,HEIGHT*.53,1.7,hero.dir,moving)
 box(0,HEIGHT-90,WIDTH,90,{17,28,44})
 style.fill(255) text("TYDUS  |  DRACORIA",120,HEIGHT-38)
 button("EDIT",WIDTH-95,HEIGHT-73,78,45,false)
 local jx,jy=joy and joy.x or 80,joy and joy.y or 100
 style.fill(36,65,88,175) ellipse(jx,jy,108,108)
 style.fill(125,172,205) ellipse(jx+(joy and joy.sx*34 or 0),jy+(joy and joy.sy*34 or 0),45,45)
end
function setup()
 viewer.mode=FULLSCREEN
 mode="creator"
end
function draw()
 local dt=math.min(DeltaTime or .016,.05)
 if mode=="creator" then drawCreator() else drawGame(dt) end
end
function touched(t)
 if mode=="creator" then
  if t.state~=BEGAN then return end
  local bottom=HEIGHT*.45;local y=bottom-93;local gap=5;local w=(WIDTH-36-3*gap)/4
  if hit(t,20,20,WIDTH-40,60) then mode="game";joy=nil;return end
  if hit(t,25,bottom+8,90,40) then facing=(facing+7)%8;return end
  if hit(t,WIDTH-115,bottom+8,90,40) then facing=(facing+1)%8;return end
  for i=1,4 do if hit(t,18+(i-1)*(w+gap),y,w,48) then
   appearance[cats[selected][2]]=i;return end end
  if hit(t,18,y-61,(WIDTH-42)/2,45) then selected=(selected-2+#cats)%#cats+1;return end
  if hit(t,WIDTH/2+3,y-61,(WIDTH-42)/2,45) then selected=selected%#cats+1;return end
  return
 end
 if t.state==BEGAN then
  if hit(t,WIDTH-95,HEIGHT-73,78,45) then mode="creator";joy=nil;return end
  if t.y<HEIGHT*.3 then joy={id=t.id,x=t.x,y=t.y,sx=0,sy=0,dx=0,dy=0} end
 elseif t.state==MOVING and joy and t.id==joy.id then
  local dx,dy=t.x-joy.x,t.y-joy.y
  local d=math.sqrt(dx*dx+dy*dy)
  local power=math.min(d/45,1)
  if d>0 then
   joy.sx=dx/d*power;joy.sy=dy/d*power
   joy.dx=(joy.sx*2+joy.sy)/math.sqrt(5)
   joy.dy=(joy.sy-joy.sx*2)/math.sqrt(5)
  end
 elseif (t.state==ENDED or t.state==CANCELLED) and joy and t.id==joy.id then joy=nil end
end

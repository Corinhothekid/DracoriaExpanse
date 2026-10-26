-- DRACORIA EVOLVED | BUILD 016 | Tydus Walk + Basic Attack
-- Import TydusAtlas.png into Codea Documents assets before running.
-- Atlas: 384x1792; 4 columns x 16 rows; 96x112 cells.
-- Rows 0-7 walking; rows 8-15 attacking (TOP to BOTTOM).
-- Direction order: down, down-left, left, up-left, up, up-right, right, down-right.

local hero={x=0,y=0,dir=1,walk=0,attack=0}
local joy=nil
local frames={}
local missing=false
local directions={7,8,1,2,3,4,5,6} -- screen-angle sectors E, NE, N, NW, W, SW, S, SE
local cellW,cellH=96,112
local function iso(x,y) return (x-y)*40,(x+y)*20 end
local function tile(x,y,cx,cy)
    local px,py=iso(x,y)
    px=px-cx+WIDTH/2; py=py-cy+HEIGHT*.54
    local path=(math.abs(x)<2 or math.abs(y)<2)
    if path then style.fill(122,111,95) else
        if (x+y)%2==0 then style.fill(65,121,75) else style.fill(72,133,81) end
    end
    style.noStroke()
    triangle(px,py+20,px+40,py,px,py-20)
    triangle(px,py+20,px,py-20,px-40,py)
end
local function loadSprites()
    local ok,img=pcall(function() return readImage(asset.documents.TydusAtlas) end)
    if not ok or not img then missing=true; return end
    if img.width~=384 or img.height~=1792 then
        print('TydusAtlas must be 384 x 1792 pixels') missing=true; return
    end
    for action=1,2 do
        frames[action]={}
        for dir=1,8 do
            frames[action][dir]={}
            local row=(action-1)*8+(dir-1)
            for f=1,4 do
                frames[action][dir][f]=img:copy((f-1)*cellW,(15-row)*cellH,cellW,cellH)
            end
        end
    end
end
function setup()
    viewer.mode=FULLSCREEN
    loadSprites()
end
local function update(dt)
    if hero.attack>0 then hero.attack=math.max(0,hero.attack-dt) end
    if not joy or hero.attack>0 then return end
    local sx,sy=joy.dx,joy.dy
    local mag=math.sqrt(sx*sx+sy*sy)
    if mag<.08 then return end
    local speed=2.7
    local wx=(sx/40+sy/20)*.5
    local wy=(sy/20-sx/40)*.5
    local wm=math.sqrt(wx*wx+wy*wy)
    if wm>0 then hero.x=hero.x+wx/wm*speed*dt;hero.y=hero.y+wy/wm*speed*dt end
    hero.walk=hero.walk+dt
    local a=math.atan(sy,sx)
    local sector=math.floor((a+math.pi/8)/(math.pi/4))%8+1
    hero.dir=directions[sector]
end
local function drawHero()
    local x,y=WIDTH/2,HEIGHT*.54
    style.fill(18,36,30,110);ellipse(x,y+3,43,14)
    if not missing and frames[1] then
        local action=hero.attack>0 and 2 or 1
        local frame=1
        if action==2 then frame=math.min(4,math.floor((.48-hero.attack)/.12)+1)
        elseif joy and math.abs(joy.dx)+math.abs(joy.dy)>.12 then frame=math.floor(hero.walk*8)%4+1 end
        local im=frames[action][hero.dir][frame]
        if im then sprite(im,x,y+43,96,112) end
    else
        style.fill(42,88,172);rect(x-14,y+6,28,37)
        style.fill(231,219,210);ellipse(x,y+51,24,23)
    end
end
local function ui()
    style.fill(16,25,42,230);rect(0,HEIGHT-94,WIDTH,94)
    style.fill(244,213,149);fontSize(19);text('DRACORIA EVOLVED',WIDTH/2,HEIGHT-27)
    style.fill(255);fontSize(14);text('TYDUS  •  BUILD 016',WIDTH/2,HEIGHT-63)
    if missing then
        style.fill(180,45,48,225);rect(12,HEIGHT-151,WIDTH-24,48)
        style.fill(255);fontSize(14);text('Import TydusAtlas.png into Documents assets',WIDTH/2,HEIGHT-127)
    end
    local jx,jy=joy and joy.x or 87,joy and joy.y or 105
    style.fill(30,53,81,170);ellipse(jx,jy,120,120)
    style.fill(136,176,216,210)
    ellipse(jx+(joy and joy.dx*37 or 0),jy+(joy and joy.dy*37 or 0),49,49)
    style.fill(135,54,67,240);ellipse(WIDTH-85,104,100,100)
    style.fill(255);fontSize(19);text('ATTACK',WIDTH-85,104)
end
function draw()
    local dt=math.min(DeltaTime or .016,.05)
    update(dt)
    background(42,90,63)
    local cx,cy=iso(hero.x,hero.y)
    local gx,gy=math.floor(hero.x),math.floor(hero.y)
    for y=gy-12,gy+12 do for x=gx-12,gx+12 do tile(x,y,cx,cy) end end
    drawHero()
    ui()
end
function touched(t)
    if t.state==BEGAN then
        if t.x>WIDTH-155 and t.y<185 then
            if hero.attack==0 then hero.attack=.48;hero.walk=0 end
            return
        end
        if t.y<HEIGHT*.33 and t.x<WIDTH*.58 then
            joy={id=t.id,x=t.x,y=t.y,dx=0,dy=0}
        end
    elseif t.state==MOVING and joy and joy.id==t.id then
        local dx,dy=t.x-joy.x,t.y-joy.y
        local m=math.sqrt(dx*dx+dy*dy)
        local strength=math.min(1,m/47)
        if m>0 then joy.dx=dx/m*strength;joy.dy=dy/m*strength end
    elseif (t.state==ENDED or t.state==CANCELLED) and joy and joy.id==t.id then
        joy=nil
    end
end

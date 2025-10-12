
public interface I_MouseClickable
{
    void OnLeftClicked();
    void OnRightClicked();
}

public interface I_Attackable
{
    void OnAttack();

    void OnAttackWithDamage(float damage);
}
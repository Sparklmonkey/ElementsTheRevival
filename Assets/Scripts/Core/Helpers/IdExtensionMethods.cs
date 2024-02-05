namespace Core.Helpers
{
    public static class IdExtensionMethods
    {
    
        public static bool IsFromHand(this ID id)
        {
            return id.field.Equals(FieldEnum.Hand);
        }
    
        public static bool IsOwnedBy(this ID id, OwnerEnum owner)
        {
            return id.owner.Equals(owner);
        }

        public static bool IsPlayerField(this ID id)
        {
            return id.field.Equals(FieldEnum.Player);
        }

        public static bool IsCreatureField(this ID id)
        {
            return id.field.Equals(FieldEnum.Creature);
        }

        public static bool IsPermanentField(this ID id)
        {
            return id.field.Equals(FieldEnum.Permanent);
        }
        
    }
}